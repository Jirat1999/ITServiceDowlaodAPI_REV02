using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdAllStations
    {
        public static List<dynamic> C_PRCaAllStations()
        {
            StringBuilder? oSql = new(); 
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"SELECT FTCode , FNStationId FROM {cmlTable.tTCNM_MASTER_Stations};");
                return oDB.C_PRCaoQuerytoListObj<dynamic>(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) 
            { 
                cConsole.C_PRCxLogError("cCmdAllStations: " + oEx.Message); return new List<dynamic>(); 
            }
        }
    }
}