using Dapper;
using ITServiceDowlaodAPI_REV02.Models.Database;
using Microsoft.Data.SqlClient;
using System.Globalization;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDatabaseService
    {
        public static async Task C_PRCxDatabaseAsync(cmlFuelPriceRoot oData)
        {
            cDatabase oDB = new cDatabase();
            cConsole.C_PRCxLogInfo(">>> Saving to Database (Local)...");

            try
            {
                DateTime dEffDate = DateTime.TryParse(oData.poResponse?.tDate, new CultureInfo("TH-th"), DateTimeStyles.None, out DateTime dPrase) ? dPrase : DateTime.Now.Date;

                var oDbStations = oDB.C_PRCaoQuerytoListObj<dynamic>(cSqlCommands.C_PRCtAllStations(), cConfig.oC_ConfigDB).
                    ToDictionary(x => ((string)x.FTCode).Trim().ToUpper(), x => (int)x.FNStationId);
                //-- Bom added
                //List<cmlTCNM_MASTER_Stations> aoStations = oDB.C_GETaQuerytoListObj<cmlTCNM_MASTER_Stations>(cSqlCommands.C_GETxAllStations_toListVersion(), cConfig.oConfigDB);

                var oDbFuelTypes = oDB.C_PRCaoQuerytoListObj<dynamic>(cSqlCommands.C_PRCtAllFuelTypes(), cConfig.oC_ConfigDB).
                    ToDictionary(x => ((string)x.FTCode).Trim().ToUpper(), x => (int)x.FNFuelTypeId);
                // Bom added
                //List<cmlTCNM_MASTER_FuelTypes> aoFuelTypes = oDB.C_GETaQuerytoListObj<cmlTCNM_MASTER_FuelTypes>(cSqlCommands.C_GETxAllFuelTypes(), cConfig.oConfigDB);

                var oDictPrices = oDB.C_PRCaoQuerytoListObj<dynamic>(cSqlCommands.C_PRCtGetPricesByDate(), cConfig.oC_ConfigDB, new { Date = dEffDate }).
                    ToDictionary(x => $"{x.nFNStationId}_{x.nFNFuelTypeId}", x => x.cFCPrice);

                using var oConn = new SqlConnection(oDB.C_PRCtDatabase(cConfig.oC_ConfigDB));
                await oConn.OpenAsync();
                using var oTrans = oConn.BeginTransaction();

                long nLogId = await oConn.QuerySingleAsync<long>(cSqlCommands.C_PRCtInsertLogStart(), new { Json = oData.tRawJson }, oTrans);
                int nStationCount = 0;

                var oListUpdateStations = new List<object>();
                var oListUpdateFuelTypes = new List<object>();
                var oListInsertPrices = new List<object>();
                var oListUpdatePrices = new List<object>();
                var oProcessedFuelTypes = new HashSet<string>();

                if (oData.poResponse?.tStations != null)
                {
                    foreach (var oStation in oData.poResponse.tStations)
                    {
                        string tStaCode = oStation.Key.Trim().ToUpper();
                        string tStaName = oStation.Key.Trim().ToUpper();

                        if (!oDbStations.TryGetValue(tStaCode, out int nStationId))
                        {
                            nStationId = await oConn.QuerySingleAsync<int>(cSqlCommands.C_PRCtInsertStation(), new { Code = tStaCode, Name = tStaName }, oTrans);
                            oDbStations[tStaCode] = nStationId;
                        }
                        else
                        {
                            oListUpdateStations.Add(new { Code = tStaCode, Name = tStaName });
                        }
                        nStationCount++;

                        if (oStation.Value != null)
                        {
                            foreach (var oFuel in oStation.Value)
                            {
                                string tFuelCode = oFuel.Key.Trim().ToUpper();
                                decimal cPrice = oFuel.Value.cNumericPrice;
                                if (cPrice <= 0) continue;

                                if (!oDbFuelTypes.TryGetValue(tFuelCode, out int nFuelTypeId))
                                {
                                    nFuelTypeId = await oConn.QuerySingleAsync<int>(cSqlCommands.C_PRCtInsertFuelType(),
                                        new
                                        {
                                            Code = tFuelCode,
                                            Name = oFuel.Value.tName
                                        },
                                        oTrans);
                                    oDbFuelTypes[tFuelCode] = nFuelTypeId;
                                    oProcessedFuelTypes.Add(tFuelCode);
                                }
                                else
                                {
                                    if (!oProcessedFuelTypes.Contains(tFuelCode))
                                    {
                                        oListUpdateFuelTypes.Add(new
                                        {
                                            Code = tFuelCode,
                                            Name = oFuel.Value.tName
                                        });
                                        oProcessedFuelTypes.Add(tFuelCode);
                                    }
                                }

                                string tPriceKey = $"{nStationId}_{nFuelTypeId}";

                                if (oDictPrices.TryGetValue(tPriceKey, out dynamic cOldPrice))
                                {
                                    if (cOldPrice != cPrice) oListUpdatePrices.Add(new
                                    {
                                        StationId = nStationId,
                                        FuelTypeId = nFuelTypeId,
                                        Date = dEffDate,
                                        Price = cPrice
                                    });
                                }
                                else
                                {
                                    oListInsertPrices.Add(new
                                    {
                                        StationId = nStationId,
                                        FuelTypeId = nFuelTypeId,
                                        Date = dEffDate,
                                        Price = cPrice
                                    });
                                }
                            }
                        }
                    }
                }

                if (oListUpdateStations.Any()) await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdateStation(), oListUpdateStations, oTrans);
                if (oListUpdateFuelTypes.Any()) await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdateFuelType(), oListUpdateFuelTypes, oTrans);
                if (oListInsertPrices.Any()) await oConn.ExecuteAsync(cSqlCommands.C_PRCtInsertPrice(), oListInsertPrices, oTrans);
                if (oListUpdatePrices.Any()) await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdatePrice(), oListUpdatePrices, oTrans);

                await oConn.ExecuteAsync(cSqlCommands.C_PRCtUpdateLogEnd(), new
                {
                    StaCount = nStationCount,
                    PriceCount = oListInsertPrices.Count,
                    LogId = nLogId
                },
                oTrans);

                oTrans.Commit();
                cConsole.C_PRCxLogProcess($">>> Database save complete! (Stations: {nStationCount}, New Prices Inserted: {oListInsertPrices.Count}, Prices Updated: {oListUpdatePrices.Count})");
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError($">>> DB Error: {oEx.Message}");
                cLog.C_PRCxLog("cDatabaseService", "C_PRCxDatabaseAsync", oEx.Message);
                await cDbLogHelper.C_PRCxLogErrorAsync(oDB.C_PRCtDatabase(cConfig.oC_ConfigDB), "C_SAVxDatabaseAsync", oEx.Message, oEx.StackTrace ?? "");
            }
        }
    }
}
