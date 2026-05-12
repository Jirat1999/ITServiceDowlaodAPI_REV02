using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdUpdateFuelType
    {
        public static bool C_PRCbUpdateFuelType(string ptCode, string ptName)
        {
            StringBuilder? oSql = new(); 
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_MASTER_FuelTypes} SET FTName = N'{ptName}', FDUpdatedAt = GETDATE() WHERE FTCode = '{ptCode}';");
                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) { cConsole.C_PRCxLogError("cCmdUpdateFuelType: " + oEx.Message); return false; }
        }
    }
}