using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdUpdateStation
    {
        public static bool C_PRCoUpdateStation(string ptCode, string ptName)
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_MASTER_Stations} SET FTName = N'{ptName}', FDUpdatedAt = GETDATE() WHERE FTCode = '{ptCode}';");
                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) 
            { 
                cConsole.C_PRCxLogError("cCmdUpdateStation: " + oEx.Message); 
                return false; 
            }
        }
    }
}