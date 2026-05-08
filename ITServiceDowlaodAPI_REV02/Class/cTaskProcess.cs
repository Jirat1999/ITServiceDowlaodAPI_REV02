using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cTaskProcess
    {
        public static async Task C_PRCxTaskProcessAsync(CancellationToken oStoppingToken)
        {
            bool bStaProcess = false;
            try
            {
                if (cConfig.aoC_Tasklist != null && cConfig.aoC_Tasklist.Count > 0)
                {
                    foreach (var oTask in cConfig.aoC_Tasklist)
                    {
                        if (oTask.nTaskActive == 1)
                        {
                            cMonitor.C_PRCbMonitor_service(cConfig.oSettingConfig.tAppCode, cConfig.oSettingConfig.tAppName, cCS.nCS_EventCode_process, oTask.tTaskCode, oTask.tTaskName);
                            cConsole.C_LogInfo($"Start Task {oTask.tTaskCode} : {oTask.tTaskName}");

                            switch (oTask.tTaskCode)
                            {
                                case "T001":
                                    bStaProcess = await C_PRCbTaskDownloadOilPrice(oStoppingToken);
                                    break;
                            }

                            if (bStaProcess) cConsole.C_LogProcess($"Success Function {oTask.tTaskCode}:{oTask.tTaskName}");
                            else cConsole.C_LogWarning($"UnSuccess Function {oTask.tTaskCode}:{oTask.tTaskName}");

                            cConsole.C_LogInfo($"End Task {oTask.tTaskCode} : {oTask.tTaskName}");
                            cConsole.C_LogInfo("---------------------------------");
                        }
                    }
                }
            }
            catch (Exception oEx) 
            { 
                cConsole.C_LogError("cTaskProcess Error : " + oEx.Message);
                cLog.C_WRTxLog("cTaskProcess", "C_PRCxTaskProcessAsync", oEx.Message);
            }
        }

        private static async Task<bool> C_PRCbTaskDownloadOilPrice(CancellationToken oStoppingToken)
        {
            try
            {
                var (oData, tJon) = await cApiService.C_GETxOilPriceAsync(oStoppingToken);
                if (oData != null && !string.IsNullOrEmpty(tJon))
                {
                    oData.tRawJson = tJon;
                    await cDatabaseService.C_SAVxDatabaseAsync(oData);
                    return true;
                }
                cConsole.C_LogWarning(">>> Skip saving: API returned null or empty data.");
                return false;
            }
            catch (Exception ex)
            {
                cConsole.C_LogError("Error in C_PRCbTaskDownloadOilPrice: " + ex.Message);
                cLog.C_WRTxLog("cTaskProcess", "C_PRCbTaskDownloadOilPrice", ex.Message);
                return false;
            }
        }
    }
}
