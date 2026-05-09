using Dapper;
using ITServiceDowlaodAPI_REV02.Models.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDatabaseService
    {
        public static async Task C_SAVxDatabaseAsync(cmlFuelPriceRoot oData)
        {
            cDatabase oDB = new cDatabase();
            cConsole.C_LogInfo(">>> Saving to Database (Local)...");

            try
            {
                DateTime dEffDate = DateTime.TryParse(oData.poResponse?.tDate, new CultureInfo("TH-th"), DateTimeStyles.None, out DateTime dPrase) ? dPrase : DateTime.Now.Date;

                var oDbStations = oDB.C_GETaQuerytoListObj<dynamic>(cSqlCommands.C_GETxAllStations(), cConfig.oConfigDB).
                    ToDictionary(x => ((string)x.FTCode).Trim().ToUpper(), x => (int)x.FNStationId);
                List<cmlTCNM_MASTER_Stations> aoStations = oDB.C_GETaQuerytoListObj<cmlTCNM_MASTER_Stations>(cSqlCommands.C_GETxAllStations_toListVersion(), cConfig.oConfigDB);

                var oDbFuelTypes = oDB.C_GETaQuerytoListObj<dynamic>(cSqlCommands.C_GETxAllFuelTypes(), cConfig.oConfigDB).
                    ToDictionary(x => ((string)x.FTCode).Trim().ToUpper(), x => (int)x.FNFuelTypeId);
                List<cmlTCNM_MASTER_FuelTypes> aoFuelTypes = oDB.C_GETaQuerytoListObj<cmlTCNM_MASTER_FuelTypes>(cSqlCommands.C_GETxAllFuelTypes(), cConfig.oConfigDB);

                var oDictPrices = oDB.C_GETaQuerytoListObj<cmlTCNM_PRICE_FuelPrices>(cSqlCommands.C_GETxPricesByDate(), cConfig.oConfigDB, new { Date = dEffDate }).
                    ToDictionary(x => $"{x.nFNStationId}_{x.nFNFuelTypeId}", x => x.cFCPrice);

                using var oConn = new SqlConnection(oDB.C_CONtDatabase(cConfig.oConfigDB));
                await oConn.OpenAsync();
                using var oTrans = oConn.BeginTransaction();

                long nLogId = await oConn.QuerySingleAsync<long>(cSqlCommands.C_GETxInsertLogStart(), new { Json = oData.tRawJson }, oTrans);
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
                            nStationId = await oConn.QuerySingleAsync<int>(cSqlCommands.C_GETxInsertStation(), new { Code = tStaCode, Name = tStaName }, oTrans);
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

                                //if (!oDbFuelTypes.TryGetValue(tFuelCode, out int nFuelTypeId)) 
                                if (!oDbFuelTypes.TryGetValue(tFuelCode, out int nFuelTypeId))
                                {
                                    nFuelTypeId = await oConn.QuerySingleAsync<int>(cSqlCommands.C_GETxInsertFuelType(), 
                                        new { 
                                            Code = tFuelCode, 
                                            Name = oFuel.Value.tName }, 
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

                                if (oDictPrices.TryGetValue(tPriceKey, out decimal cOldPrice))
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

                if (oListUpdateStations.Any()) await oConn.ExecuteAsync(cSqlCommands.C_GETxUpdateStation(), oListUpdateStations, oTrans);
                if (oListUpdateFuelTypes.Any()) await oConn.ExecuteAsync(cSqlCommands.C_GETxUpdateFuelType(), oListUpdateFuelTypes, oTrans);
                if (oListInsertPrices.Any()) await oConn.ExecuteAsync(cSqlCommands.C_GETxInsertPrice(), oListInsertPrices, oTrans);
                if (oListUpdatePrices.Any()) await oConn.ExecuteAsync(cSqlCommands.C_GETxUpdatePrice(), oListUpdatePrices, oTrans);

                await oConn.ExecuteAsync(cSqlCommands.C_GETxUpdateLogEnd(), new 
                { 
                    StaCount = nStationCount, 
                    PriceCount = oListInsertPrices.Count, 
                    LogId = nLogId 
                }, 
                oTrans);

                oTrans.Commit();
                cConsole.C_LogProcess($">>> Database save complete! (Stations: {nStationCount}, New Prices Inserted: {oListInsertPrices.Count}, Prices Updated: {oListUpdatePrices.Count})");
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError($">>> DB Error: {oEx.Message}");
                cLog.C_WRTxLog("cDatabaseService", "C_SAVxDatabaseAsync", oEx.Message);
                await cDbLogHelper.C_SAVxLogErrorAsync(oDB.C_CONtDatabase(cConfig.oConfigDB), "C_SAVxDatabaseAsync", oEx.Message, oEx.StackTrace ?? "");
            }
        }
    }
}
