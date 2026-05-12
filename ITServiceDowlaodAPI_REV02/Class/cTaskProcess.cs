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
                            cMonitor.C_PRCbMonitor_service(cConfig.oC_SettingConfig.tAppCode, cConfig.oC_SettingConfig.tAppName, cCS.nCS_EventCode_process, oTask.tTaskCode, oTask.tTaskName);
                            cConsole.C_PRCxLogInfo($"Start Task {oTask.tTaskCode} : {oTask.tTaskName}");

                            switch (oTask.tTaskCode)
                            {
                                case "T001":
                                    bStaProcess = await C_PRCbTaskDownloadOilPrice(oStoppingToken);
                                    break;
                            }

                            if (bStaProcess) cConsole.C_PRCxLogProcess($"Success Function {oTask.tTaskCode}:{oTask.tTaskName}");
                            else cConsole.C_PRCxLogWarning($"UnSuccess Function {oTask.tTaskCode}:{oTask.tTaskName}");

                            cConsole.C_PRCxLogInfo($"End Task {oTask.tTaskCode} : {oTask.tTaskName}");
                            cConsole.C_PRCxLogInfo("---------------------------------");
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cTaskProcess Error : " + oEx.Message);
                cLog.C_PRCxLog("cTaskProcess", "C_PRCxTaskProcessAsync", oEx.Message);
            }
        }

        private static async Task<bool> C_PRCbTaskDownloadOilPrice(CancellationToken oStoppingToken)
        {
            try
            {
                var (oData, tJon) = await cApiService.C_PRCtOilPriceAsync(oStoppingToken);
                if (oData != null && !string.IsNullOrEmpty(tJon))
                {
                    oData.tRawJson = tJon;
                    await new cDatabaseService().cSaveToDatabaseAsync(oData);
                    return true;
                }
                cConsole.C_PRCxLogWarning(">>> Skip saving: API returned null or empty data.");
                return false;
            }
            catch (Exception ex)
            {
                cConsole.C_PRCxLogError("Error in C_PRCbTaskDownloadOilPrice: " + ex.Message);
                cLog.C_PRCxLog("cTaskProcess", "C_PRCbTaskDownloadOilPrice", ex.Message);
                return false;
            }
        }
    }
}
