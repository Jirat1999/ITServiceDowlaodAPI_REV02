using ITServiceDowlaodAPI_REV02.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdAllStations
    {
        public static List<cmlTCNM_MASTER_Stations> C_PRCaAllStations()
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"SELECT FTCode AS tFTCode, FNStationId AS nFNStationId ");
                oSql.AppendLine($"FROM {cmlTable.tTCNM_MASTER_Stations};");

                return oDB.C_PRCaoQuerytoListObj<cmlTCNM_MASTER_Stations>(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cCmdAllStations: " + oEx.Message);
                return new List<cmlTCNM_MASTER_Stations>();
            }
        }
    }
}