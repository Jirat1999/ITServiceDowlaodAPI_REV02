namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cConsole
    {
        public static void C_PRCxLogMonitorConfig()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("+++++++++++++++++++++++++++++");
            Console.WriteLine("Application Config");
            Console.WriteLine("  App Mode : " + cConfig.oC_SettingConfig.tMode);
            Console.WriteLine("  Database : " + cConfig.oC_ConfigDB.tServerDB);
            Console.WriteLine("+++++++++++++++++++++++++++++\n");
            C_PRCxLogInfo("Load Config Success");
        }
        public static void C_PRCxLogProcess(string ptMsg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Log: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor();
        }
        public static void C_PRCxLogInfo(string ptMsg)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Info: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor();
        }
        public static void C_PRCxLogWarning(string ptMsg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor();
        }
        public static void C_PRCxLogEvent(string ptMsg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine($"Event: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor();
        }
        public static void C_PRCxLogError(string ptMsg)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Error: {DateTime.Now:yyyy-MM-dd HH:mm:ss} --> {ptMsg}"); Console.ResetColor();
        }
    }
}
