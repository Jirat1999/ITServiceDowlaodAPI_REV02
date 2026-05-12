using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdInsertErrorLogs
    {
        public static bool C_PRCbInsertErrorLogs(string ptProc, string ptMsg, string ptStack)
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                string tSafeProc = ptProc?.Replace("'", "''") ?? "";
                string tSafeMsg = ptMsg?.Replace("'", "''") ?? "";
                string tSafeStack = ptStack?.Replace("'", "''") ?? "";
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_ERROR_ErrorLogs} (FTProcessName, FTErrorMessage, FTStackTrace)");
                oSql.AppendLine($"VALUES (N'{tSafeProc}', N'{tSafeMsg}', N'{tSafeStack}');");
                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) { cConsole.C_PRCxLogError("cCmdInsertErrorLogs: " + oEx.Message); return false; }
        }
    }
}