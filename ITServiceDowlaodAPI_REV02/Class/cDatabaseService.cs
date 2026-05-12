using Dapper;
using ITServiceDowlaodAPI_REV02.Models.Database;
using Microsoft.Data.SqlClient;
using System.Globalization;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDatabaseService
    {
        public async Task cSaveToDatabaseAsync(cmlFuelPriceRoot oData)
        {
            string tConnStr = new cDatabase().C_PRCtDatabase(cConfig.oC_ConfigDB);
            cConsole.C_PRCxLogInfo(">>> Saving to Database...");

            try
            {
                // แปลงวันที่
                string tDateStr = oData.poResponse?.tDate ?? DateTime.Now.ToString("yyyy-MM-dd");
                DateTime dEffDate = DateTime.TryParse(tDateStr, new CultureInfo("TH-th"), DateTimeStyles.None, out DateTime dPrase) ? dPrase : DateTime.Now.Date;
                string tFormattedDate = dEffDate.ToString("yyyy-MM-dd");

                using var oConn = new SqlConnection(tConnStr);
                await oConn.OpenAsync();
                using var oTrans = oConn.BeginTransaction();

                // 1. บันทึก Log Start
                long nLogId = await oConn.QuerySingleAsync<long>(cSqlCommands.C_PRCtInsertLogStart(oData.tRawJson ?? ""), transaction: oTrans);
                int nStationCount = 0;
                int nPriceCount = 0;

                // ดึงข้อมูล Master ที่มีอยู่แล้วมาเช็ค
                var oDbStations = oConn.Query<dynamic>(cSqlCommands.C_PRCtAllStations(), transaction: oTrans)
                                       .ToDictionary(x => ((string)x.FTCode).Trim().ToUpper(), x => (int)x.FNStationId);
                var oDbFuelTypes = oConn.Query<dynamic>(cSqlCommands.C_PRCtAllFuelTypes(), transaction: oTrans)
                                        .ToDictionary(x => ((string)x.FTCode).Trim().ToUpper(), x => (int)x.FNFuelTypeId);

                // ดึงราคาของวันนี้มาเช็คว่ามีหรือยัง
                var oDictPrices = oConn.Query<cmlTCNM_PRICE_FuelPrices>(cSqlCommands.C_PRCtGetPricesByDate(tFormattedDate), transaction: oTrans)
                                       .ToDictionary(x => $"{x.nFNStationId}_{x.nFNFuelTypeId}", x => x.cFCPrice);

                var oProcessedFuelTypes = new HashSet<string>();

                if (oData.poResponse?.tStations != null)
                {
                    foreach (var oStation in oData.poResponse.tStations)
                    {
                        string tStaCode = oStation.Key.Trim().ToUpper();
                        string tStaName = oStation.Key.Trim();

                        // เช็คปั๊มน้ำมัน
                        if (!oDbStations.TryGetValue(tStaCode, out int nStationId))
                        {
                            nStationId = await oConn.QuerySingleAsync<int>(cSqlCommands.C_PRCtInsertStation(tStaCode, tStaName), transaction: oTrans);
                            oDbStations[tStaCode] = nStationId;
                        }
                        else
                        {
                            // ถ้ามีแล้ว ให้อัปเดตชื่อ
                            await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdateStation(tStaCode, tStaName), transaction: oTrans);
                        }

                        nStationCount++;

                        if (oStation.Value != null)
                        {
                            foreach (var oFuel in oStation.Value)
                            {
                                string tFuelCode = oFuel.Key.Trim().ToUpper();
                                string tFuelName = oFuel.Value.tName ?? "";
                                decimal cPrice = oFuel.Value.cNumericPrice;

                                if (cPrice <= 0) continue;

                                // เช็คชนิดน้ำมัน
                                if (!oDbFuelTypes.TryGetValue(tFuelCode, out int nFuelTypeId))
                                {
                                    nFuelTypeId = await oConn.QuerySingleAsync<int>(cSqlCommands.C_PRCtInsertFuelType(tFuelCode, tFuelName), transaction: oTrans);
                                    oDbFuelTypes[tFuelCode] = nFuelTypeId;
                                    oProcessedFuelTypes.Add(tFuelCode);
                                }
                                else
                                {
                                    if (!oProcessedFuelTypes.Contains(tFuelCode))
                                    {
                                        await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdateFuelType(tFuelCode, tFuelName), transaction: oTrans);
                                        oProcessedFuelTypes.Add(tFuelCode);
                                    }
                                }

                                string tPriceKey = $"{nStationId}_{nFuelTypeId}";

                                // เช็คราคาน้ำมัน
                                if (oDictPrices.TryGetValue(tPriceKey, out decimal cOldPrice))
                                {
                                    if (cOldPrice != cPrice)
                                    {
                                        await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdatePrice(nStationId, nFuelTypeId, tFormattedDate, cPrice), transaction: oTrans);
                                    }
                                }
                                else
                                {
                                    await oConn.ExecuteAsync(cSqlCommands.C_PRCtInsertPrice(nStationId, nFuelTypeId, tFormattedDate, cPrice), transaction: oTrans);
                                    nPriceCount++;
                                }
                            }
                        }
                    }
                }

                // 2. บันทึก Log End
                await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdateLogEnd(nLogId, nStationCount, nPriceCount), transaction: oTrans);

                oTrans.Commit();
                cConsole.C_PRCxLogProcess($">>> Database save complete! (Stations: {nStationCount}, New Prices: {nPriceCount})");
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError($">>> DB Error: {oEx.Message}");
                cLog.C_PRCxLog("cDatabaseService", "cSaveToDatabaseAsync", oEx.Message);

                // บันทึก Error ลง Database โดยส่งค่าเข้าตรงๆ
                using var oErrConn = new SqlConnection(tConnStr);
                await oErrConn.OpenAsync();
                await oErrConn.ExecuteAsync(cSqlCommands.C_PRCtInsertErrorLogs("cSaveToDatabaseAsync", oEx.Message, oEx.StackTrace ?? ""));
            }
        }
    }
}
