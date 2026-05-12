using ITServiceDowlaodAPI_REV02.Class.CommandDB;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDatabaseService
    {
        /// <summary>
        /// ฟังก์ชันหลักในการบันทึกข้อมูลราคาน้ำมันลงฐานข้อมูล
        /// </summary>
        public async Task C_PRCxDataInsertOilPriceAsync(cmlFuelPriceRoot poData)
        {
            cConsole.C_PRCxLogInfo(">>> Saving to Database (Advanced Command Pattern)...");

            try
            {
                cSP oSP = new cSP();
                string tFormattedDate = oSP.C_GETtFormattedDate(poData);

                long nLogId = cCmdInsertLogStart.C_PRCnInsertLogStart(poData.tRawJson ?? "");

                int nStationCount = 0;
                int nPriceCount = 0;

                Dictionary<string, int> oDbStations = oSP.C_GEToLoadStations();
                Dictionary<string, int> oDbFuelTypes = oSP.C_GEToLoadFuelTypes();
                Dictionary<string, decimal> oDictPrices = oSP.C_GEToLoadPrices(tFormattedDate);

                HashSet<string> oProcessedFuelTypes = new HashSet<string>();

                if (poData.poResponse?.tStations != null)
                {
                    foreach (var oStation in poData.poResponse.tStations)
                    {
                        string tStaCode = oStation.Key.Trim().ToUpper();
                        string tStaName = oStation.Key.Trim();

                        if (!oDbStations.TryGetValue(tStaCode, out int nStationId))
                        {
                            nStationId = cCmdInsertStation.C_PRCnInsertStation(tStaCode, tStaName);
                            oDbStations[tStaCode] = nStationId;
                        }
                        else
                        {
                            cCmdUpdateStation.C_PRCbUpdateStation(tStaCode, tStaName);
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

                                if (!oDbFuelTypes.TryGetValue(tFuelCode, out int nFuelTypeId))
                                {
                                    nFuelTypeId = cCmdInsertFuelType.C_PRCnInsertFuelType(tFuelCode, tFuelName);
                                    oDbFuelTypes[tFuelCode] = nFuelTypeId;
                                    oProcessedFuelTypes.Add(tFuelCode);
                                }
                                else
                                {
                                    if (!oProcessedFuelTypes.Contains(tFuelCode))
                                    {
                                        cCmdUpdateFuelType.C_PRCbUpdateFuelType(tFuelCode, tFuelName);
                                        oProcessedFuelTypes.Add(tFuelCode);
                                    }
                                }

                                string tPriceKey = $"{nStationId}_{nFuelTypeId}";

                                if (oDictPrices.TryGetValue(tPriceKey, out decimal cOldPrice))
                                {
                                    if (cOldPrice != cPrice)
                                    {
                                        cCmdUpdatePrice.C_PRCbUpdatePrice(nStationId, nFuelTypeId, tFormattedDate, cPrice);
                                    }
                                }
                                else
                                {
                                    cCmdInsertPrice.C_PRCbInsertPrice(nStationId, nFuelTypeId, tFormattedDate, cPrice);
                                    nPriceCount++;
                                }
                            }
                        }
                    }
                }

                cCmdUpdateLogEnd.C_PRCbUpdateLogEnd(nLogId, nStationCount, nPriceCount);

                cConsole.C_PRCxLogProcess($">>> Database save complete! (Stations: {nStationCount}, New Prices: {nPriceCount})");
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError($">>> DB Error: {oEx.Message}");
                cLog.C_PRCxLog("cDatabaseService", "C_PRCxDataInsertOilPriceAsync", oEx.Message);

                cCmdInsertErrorLogs.C_PRCbInsertErrorLogs("C_PRCxDataInsertOilPriceAsync", oEx.Message, oEx.StackTrace ?? "");
            }
        }
    }
}