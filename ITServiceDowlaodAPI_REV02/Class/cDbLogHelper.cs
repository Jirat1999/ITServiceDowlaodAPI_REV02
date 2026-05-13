using ITServiceDowlaodAPI_REV02.Class.CommandDB; 

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDbLogHelper
    {
        public static async Task C_PRCxLogErrorAsync(string tConnStr, string tProcess, string tMsg, string tStackTrace)
        {
            try
            {
                cCmdInsertErrorLogs.C_PRCbInsertErrorLogs(tProcess, tMsg, tStackTrace);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError($">>> CRITICAL ERROR: {oEx.Message}");
            }
        }
    }
}