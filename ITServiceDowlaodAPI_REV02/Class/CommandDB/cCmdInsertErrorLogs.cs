using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdInsertErrorLogs
    {
        public static bool C_PRCbInsertErrorLogs(cmlTCNM_ERROR_ErrorLogs poErrorData)
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                string tSafeProc = poErrorData.tFTProcessName?.Replace("'", "''") ?? "";
                string tSafeMsg = poErrorData.tFTErrorMessage?.Replace("'", "''") ?? "";
                string tSafeStack = poErrorData.tFTStackTrace?.Replace("'", "''") ?? "";

                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_ERROR_ErrorLogs} (FTProcessName, FTErrorMessage, FTStackTrace)");
                oSql.AppendLine($"VALUES (N'{tSafeProc}', N'{tSafeMsg}', N'{tSafeStack}');");

                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cCmdInsertErrorLogs: " + oEx.Message);
                return false;
            }
        }
    }
}