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
                cConsole.C_LogMonitorConfig();

                cMonitor.C_PRCbMonitor_service(cConfig.oSettingConfig.tAppCode, cConfig.oSettingConfig.tAppName, cCS.nCS_EventCode_start, string.Empty, string.Empty);

                while (!stoppingToken.IsCancellationRequested)
                {
                    int nCyCleTime = cConfig.oSettingConfig.nCycleTime <= 0 ? 5 : cConfig.oSettingConfig.nCycleTime;

                    cConsole.C_LogEvent($"Start Process... ({nCyCleTime})min");

                    // 1. สั่งประมวลผลงาน (ดึงผ่าน cTaskProcess แบบ Static)
                    await cTaskProcess.C_PRCxTaskProcessAsync(stoppingToken);

                    // 2. เช็คโหมด Manual Trigger
                    if (cConfig.oSettingConfig.bManualTrigger)
                    {
                        cConsole.C_LogInfo("💡 สั่งรันแบบ Manual Trigger 1 รอบ สำเร็จ!");

                        // 3. สั่งให้เขียนแก้ไฟล์ appsettings.json กลับเป็น false
                        cConfig.C_SETbManualTrigger(false);

                        cConsole.C_LogInfo("🔧 ระบบแก้ไฟล์ appsettings.json เด้งกลับเป็น false ให้อัตโนมัติแล้ว");
                    }

                    // 4. ไหลมาเข้าสู่โหมดสแตนด์บาย 5 นาที ตามปกติ (ไม่หยุดทำงาน)
                    cMonitor.C_PRCbMonitor_service(cConfig.oSettingConfig.tAppCode, cConfig.oSettingConfig.tAppName, cCS.nCS_EventCode_sleep, string.Empty, string.Empty);

                    cConsole.C_LogEvent($"Service working... Waiting for the next round in {nCyCleTime} minutes...\n");
                    await Task.Delay(TimeSpan.FromMinutes(nCyCleTime), stoppingToken);
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("Worker Error : " + oEx.Message);
                cLog.C_WRTxLog("Worker", "ExecuteAsync", oEx.Message);
            }
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            cConsole.C_LogWarning("Application is exiting...");
            cMonitor.C_PRCbMonitor_service(cConfig.oSettingConfig.tAppCode, cConfig.oSettingConfig.tAppName, cCS.nCS_EventCode_exit, string.Empty, string.Empty);
        }

        public override void Dispose()
        {
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            base.Dispose();
        }
    }
}