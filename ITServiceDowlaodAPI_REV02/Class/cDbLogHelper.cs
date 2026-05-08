using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDbLogHelper
    {
        public static async Task C_SAVxLogErrorAsync(string tConnStr, string tProcess, string tMsg, string tStackTrace)
        {
            if (string.IsNullOrEmpty(tConnStr)) return;
            try
            {
                using var oConnErr = new SqlConnection(tConnStr);
                await oConnErr.ExecuteAsync(cSqlCommands.C_GETxInsertErrorLogs(), new { Proc = tProcess, Msg = tMsg, Stack = tStackTrace });
            }
            catch (Exception oEx) { cConsole.C_LogError($">>> CRITICAL ERROR: {oEx.Message}"); }
        }
    }
}
