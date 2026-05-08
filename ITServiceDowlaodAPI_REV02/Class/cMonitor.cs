using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cMonitor
    {
        public static bool C_PRCbMonitor_service(string? ptApp_code, string? ptApp_name, int pnEventCode, string? ptTaskCode, string? ptTask_name)
        {
            try
            {
                cDatabase oDB = new cDatabase();
                string tSubAppCode = $"{ptApp_code}_{ptApp_name}";
                string tServer = $"{cCS.tCS_IPLocal}|{cCS.tCS_HostName}";

                // ใช้ StringBuilder ในการต่อคำสั่ง SQL เพื่อให้อ่านง่ายและเป็นระเบียบ
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"IF EXISTS (SELECT 1 FROM TSDC_MONITOR_SERVICE WHERE FTApp_code = '{ptApp_code}' AND FTSubApp_code = '{tSubAppCode}')");
                oSql.AppendLine($"UPDATE TSDC_MONITOR_SERVICE SET FNEvent_code = {pnEventCode}, FTTask_code = '{ptTaskCode}', FDLastupdate = GETDATE()");
                oSql.AppendLine($"WHERE FTApp_code = '{ptApp_code}' AND FTSubApp_code = '{tSubAppCode}'");
                oSql.AppendLine($"ELSE");
                oSql.AppendLine($"INSERT INTO TSDC_MONITOR_SERVICE (FTApp_code, FTApp_name, FTSubApp_code, FNEvent_code, FTTask_code, FTServer, FDCreatedate, FDLastupdate)");
                oSql.AppendLine($"VALUES ('{ptApp_code}', '{ptApp_name}', '{tSubAppCode}', {pnEventCode}, '{ptTaskCode}', '{tServer}', GETDATE(), GETDATE());");

                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oConfigDB);
            }
            catch { return false; }
        }
    }
}
