using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdUpdateLogEnd
    {
        public static bool C_PRCoUpdateLogEnd(cmlTCNM_LOG_FuelUpdate poLogData)
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                string tSafeMsg = poLogData.tFTMeassage?.Replace("'", "''") ?? "";

                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_LOG_FuelUpdate}");
                oSql.AppendLine($"SET FDUpdateEnd = GETDATE(), ");
                oSql.AppendLine($"FNStationCount = {poLogData.nFNStationCount ?? 0}, ");
                oSql.AppendLine($"FNPriceCount = {poLogData.nFNPriceCount ?? 0}, ");
                oSql.AppendLine($"FTStatus = '{poLogData.tFTStatus}', ");
                oSql.AppendLine($"FTMessage = N'{tSafeMsg}' ");
                oSql.AppendLine($"WHERE FNLogId = {poLogData.nFNLogId};");

                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cCmdUpdateLogEnd: " + oEx.Message);
                return false;
            }
        }
    }
}