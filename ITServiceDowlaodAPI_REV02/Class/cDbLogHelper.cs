using ITServiceDowlaodAPI_REV02.Class.CommandDB;
using ITServiceDowlaodAPI_REV02.Models.Database;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDbLogHelper
    {
        public static async Task C_PRCxLogErrorAsync(string tConnStr, string tProcess, string tMsg, string tStackTrace)
        {
            try
            {
                cmlTCNM_ERROR_ErrorLogs oErrorLog = new cmlTCNM_ERROR_ErrorLogs()
                {
                    tFTProcessName = tProcess,
                    tFTErrorMessage = tMsg,
                    tFTStackTrace = tStackTrace
                };

                cCmdInsertErrorLogs.C_PRCbInsertErrorLogs(oErrorLog);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError($">>> CRITICAL ERROR: {oEx.Message}");
            }
        }
    }
}