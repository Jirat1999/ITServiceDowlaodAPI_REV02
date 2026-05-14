using ITServiceDowlaodAPI_REV02.Class.CommandDB;
using ITServiceDowlaodAPI_REV02.Models.Database;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDatabaseService
    {
        public async Task C_PRCxDataInsertOilPriceAsync(cmlFuelPriceRoot poData)
        {
            cConsole.C_PRCxLogInfo(">>> Saving to Database (Advanced Command Pattern)...");

            try
            {
                cSP oSP = new cSP();
                string tFormattedDate = oSP.C_PRCtFormattedDate(poData);

                // =========================================================
                // 1. สร้าง Model เก็บ Log และบันทึกเริ่มต้น (Start)
                // =========================================================
                cmlTCNM_LOG_FuelUpdate oLogData = new cmlTCNM_LOG_FuelUpdate()
                {
                    tFTStatus = "Processing",
                    tFTPriceDataJSON = poData.tRawJson ?? ""
                };

                oLogData.nFNLogId = cCmdInsertLogStart.C_PRCnInsertLogStart(oLogData);

                int nStationCount = 0;
                int nPriceCount = 0;

                // =========================================================
                // 2. โหลด Master Data มาเตรียมไว้
                // =========================================================
                Dictionary<string, int> oDbStations = oSP.C_PRCoLoadStations();
                Dictionary<string, int> oDbFuelTypes = oSP.C_PRCaoLoadFuelTypes();
                Dictionary<string, decimal> oDictPrices = oSP.C_PRCaoLoadPrices(tFormattedDate);

                HashSet<string> tProcessedFuelTypes = new HashSet<string>();

                // =========================================================
                // 3. เริ่มลูปข้อมูลหลัก
                // =========================================================
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
                                    tProcessedFuelTypes.Add(tFuelCode);
                                }
                                else
                                {
                                    if (!tProcessedFuelTypes.Contains(tFuelCode))
                                    {
                                        cCmdUpdateFuelType.C_PRCoUpdateFuelType(tFuelCode, tFuelName);
                                        tProcessedFuelTypes.Add(tFuelCode);
                                    }
                                }

                                string tPriceKey = $"{nStationId}_{nFuelTypeId}";

                                if (oDictPrices.TryGetValue(tPriceKey, out decimal cOldPrice))
                                {
                                    if (cOldPrice != cPrice)
                                    {
                                        cCmdUpdatePrice.C_PRCoUpdatePrice(nStationId, nFuelTypeId, tFormattedDate, cPrice);
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

                // =========================================================
                // 4. สรุปผลและบันทึก Log ตอนจบ (End)
                // =========================================================
                oLogData.nFNStationCount = nStationCount;
                oLogData.nFNPriceCount = nPriceCount;
                oLogData.tFTStatus = "Success";
                oLogData.tFTMeassage = "Complete";

                cCmdUpdateLogEnd.C_PRCoUpdateLogEnd(oLogData);

                cConsole.C_PRCxLogProcess($">>> Database save complete! (Stations: {nStationCount}, New Prices: {nPriceCount})");
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError($">>> DB Error: {oEx.Message}");
                cLog.C_PRCxLog("cDatabaseService", "C_PRCxDataInsertOilPriceAsync", oEx.Message);

                cmlTCNM_ERROR_ErrorLogs oErrorLog = new cmlTCNM_ERROR_ErrorLogs()
                {
                    tFTProcessName = "C_PRCxDataInsertOilPriceAsync",
                    tFTErrorMessage = oEx.Message,
                    tFTStackTrace = oEx.StackTrace ?? ""
                };

                cCmdInsertErrorLogs.C_PRCbInsertErrorLogs(oErrorLog);
            }
        }
    }
}