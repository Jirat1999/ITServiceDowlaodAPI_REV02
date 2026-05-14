using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cMonitor
    {
        public static bool C_PRCbMonitor_service(string? tApp_code, string? tApp_name, int nEventCode, string? tTaskCode, string? tTask_name)
        {
            try
            {
                cDatabase oDB = new cDatabase();
                string tSubAppCode = $"{tApp_code}_{tApp_name}";
                string tServer = $"{cCS.tCS_IPLocal}|{cCS.tCS_HostName}";

                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"IF EXISTS (SELECT 1 FROM {cmlTable.tTSDC_MONITOR_SERVICE} WHERE FTApp_code = '{tApp_code}' AND FTSubApp_code = '{tSubAppCode}')");
                oSql.AppendLine($"UPDATE {cmlTable.tTSDC_MONITOR_SERVICE} SET FNEvent_code = {nEventCode}, FTTask_code = '{tTaskCode}', FDLastupdate = GETDATE()");
                oSql.AppendLine($"WHERE FTApp_code = '{tApp_code}' AND FTSubApp_code = '{tSubAppCode}'");
                oSql.AppendLine($"ELSE");
                oSql.AppendLine($"INSERT INTO {cmlTable.tTSDC_MONITOR_SERVICE} (FTApp_code, FTApp_name, FTSubApp_code, FNEvent_code, FTTask_code, FTServer, FDCreatedate, FDLastupdate)");
                oSql.AppendLine($"VALUES ('{tApp_code}', '{tApp_name}', '{tSubAppCode}', {nEventCode}, '{tTaskCode}', '{tServer}', GETDATE(), GETDATE());");

                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cMonitor.C_PRCbMonitor_service: " + oEx.Message);
                return false;
            }
        }

        public static bool C_PRCoMonitor_service(string? tApp_code, string? tApp_name, int nEventCode, string? tTaskCode, string? tTask_name)
        {
            try
            {
                return C_PRCbMonitor_service(tApp_code, tApp_name, nEventCode, tTaskCode, tTask_name);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cMonitor.C_PRCoMonitor_service: " + oEx.Message);
                return false; 
            }
        }
    }
}