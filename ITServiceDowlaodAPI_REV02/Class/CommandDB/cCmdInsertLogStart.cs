using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdInsertLogStart
    {
        public static long C_PRCnInsertLogStart(string ptJson)
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                string tSafeJson = ptJson?.Replace("'", "''") ?? "";
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_LOG_FuelUpdate} (FDUpdateStart, FTStatus, FTPriceDataJSON)");
                oSql.AppendLine($"VALUES (GETDATE(), 'Processing', N'{tSafeJson}');");
                oSql.AppendLine($"SELECT CAST(SCOPE_IDENTITY() as BIGINT);");
                var aRes = oDB.C_PRCaoQuerytoListObj<long>(oSql.ToString(), cConfig.oC_ConfigDB);
                return (aRes != null && aRes.Count > 0) ? aRes[0] : 0;
            }
            catch (Exception oEx) 
            { 
                cConsole.C_PRCxLogError("cCmdInsertLogStart: " + oEx.Message); 
                return 0; 
            }
        }
    }
}