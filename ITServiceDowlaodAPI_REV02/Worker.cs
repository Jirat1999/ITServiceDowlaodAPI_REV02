using ITServiceDowlaodAPI_REV02.Class;

namespace ITServiceDowlaodAPI_REV02
{
    public class Worker : BackgroundService
    {
        public Worker()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Console.Clear();
                cConsole.C_PRCxLogMonitorConfig();

                cMonitor.C_PRCoMonitor_service(cConfig.oC_SettingConfig.tAppCode, cConfig.oC_SettingConfig.tAppName, cCS.nCS_EventCode_start, string.Empty, string.Empty);

                while (!stoppingToken.IsCancellationRequested)
                {
                    int nCyCleTime = cConfig.oC_SettingConfig.nCycleTime <= 0 ? 5 : cConfig.oC_SettingConfig.nCycleTime;

                    cConsole.C_PRCxLogEvent($"Start Process... ({nCyCleTime})min");

                    await cTaskProcess.C_PRCxTaskProcessAsync(stoppingToken);

                    if (cConfig.oC_SettingConfig.bManualTrigger)
                    {
                        cConsole.C_PRCxLogInfo("💡 Running Manual Trigger 1 Complete");

                        cConfig.C_PRCxSetManualTrigger(false);

                        cConsole.C_PRCxLogInfo("🔧 The system has automatically reverted the appsettings.json file to false");
                    }

                    cMonitor.C_PRCbMonitor_service(cConfig.oC_SettingConfig.tAppCode, cConfig.oC_SettingConfig.tAppName, cCS.nCS_EventCode_sleep, string.Empty, string.Empty);

                    cConsole.C_PRCxLogEvent($"Service working... Waiting for the next round in {nCyCleTime} minutes...\n");
                    await Task.Delay(TimeSpan.FromMinutes(nCyCleTime), stoppingToken);
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("Worker Error : " + oEx.Message);
                cLog.C_PRCxLog("Worker", "ExecuteAsync", oEx.Message);
            }
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            cConsole.C_PRCxLogWarning("Application is exiting...");
            cMonitor.C_PRCbMonitor_service(cConfig.oC_SettingConfig.tAppCode, cConfig.oC_SettingConfig.tAppName, cCS.nCS_EventCode_exit, string.Empty, string.Empty);
        }

        public override void Dispose()
        {
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            base.Dispose();
        }
    }
}