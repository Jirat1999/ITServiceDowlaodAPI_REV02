using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdUpdateLogEnd
    {
        public static bool C_PRCbUpdateLogEnd(long pnLogId, int pnStaCount, int pnPriceCount)
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_LOG_FuelUpdate}");
                oSql.AppendLine($"SET FDUpdateEnd = GETDATE(), FNStationCount = {pnStaCount}, FNPriceCount = {pnPriceCount}, FTStatus = 'Success', FTMessage = 'Complete'");
                oSql.AppendLine($"WHERE FNLogId = {pnLogId};");
                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) { cConsole.C_PRCxLogError("cCmdUpdateLogEnd: " + oEx.Message); return false; }
        }
    }
}