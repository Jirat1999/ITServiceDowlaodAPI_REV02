using Dapper;
using Microsoft.Data.SqlClient;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDbLogHelper
    {
        public static async Task C_PRCxLogErrorAsync(string tConnStr, string tProcess, string tMsg, string tStackTrace)
        {
            if (string.IsNullOrEmpty(tConnStr)) return;
            try
            {
                using var oConnErr = new SqlConnection(tConnStr);
                await oConnErr.ExecuteAsync(cSqlCommands.C_PRCtInsertErrorLogs(tProcess, tMsg, tStackTrace));
            }
            catch (Exception oEx) { cConsole.C_PRCxLogError($">>> CRITICAL ERROR: {oEx.Message}"); }
        }
    }
}
