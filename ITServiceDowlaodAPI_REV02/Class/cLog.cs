namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cLog
    {
        private static readonly object poLockObj = new object();
        public static void C_PRCxLog(string ptClass, string ptFunction, string ptMessage, bool pbIsError = false)
        {
            try
            {
                string tPath = AppDomain.CurrentDomain.BaseDirectory + "Log";

                if (!Directory.Exists(tPath))
                {
                    Directory.CreateDirectory(tPath);
                }

                string tFileName = tPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                string tLogMsg = $"{DateTime.Now:HH:mm:ss} | Class: {ptClass} | Func: {ptFunction} | Msg: {ptMessage}";

                lock (poLockObj)
                {
                    using (StreamWriter w = File.AppendText(tFileName))
                    {
                        w.WriteLine(tLogMsg);
                    }
                }

                using (StreamWriter w = File.AppendText(tFileName))
                {
                    w.WriteLine(tLogMsg);
                }

                string tConnString = new cDatabase().C_PRCtDatabase(cConfig.oC_ConfigDB);
                if (!string.IsNullOrEmpty(tConnString))
                {
                    if (pbIsError)
                    {
                        _ = cDbLogHelper.C_PRCxLogErrorAsync(tConnString, $"{ptClass}.{ptFunction}", ptMessage, "");
                    }
                    else
                    {
                        // เรียกใช้ฟังก์ชันที่จัดการตาราง Activity Events ได้ที่นี่
                        // _ = cDbLogHelper.C_PRCxLogEventAsync(tConnString, $"{ptClass}.{ptFunction}", ptMessage);
                    }
                }
            }
            catch (Exception oEx)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[CRITICAL LOG FAILED]: {oEx.Message}");
                Console.ResetColor();
            }
        }
    }
}
