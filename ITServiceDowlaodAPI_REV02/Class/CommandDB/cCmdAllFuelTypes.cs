using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdAllFuelTypes
    {
        public static List<dynamic> C_PRCaAllFuelTypes()
        {
            StringBuilder? oSql = new(); 
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"SELECT FTCode, FNFuelTypeId FROM {cmlTable.tTCNM_MASTER_FuelTypes};");
                return oDB.C_PRCaoQuerytoListObj<dynamic>(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) 
            { 
                cConsole.C_PRCxLogError("cCmdAllFuelTypes: " + oEx.Message); return new List<dynamic>(); 
            }
        }
    }
}