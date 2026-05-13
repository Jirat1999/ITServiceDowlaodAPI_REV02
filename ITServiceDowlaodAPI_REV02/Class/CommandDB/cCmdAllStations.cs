using ITServiceDowlaodAPI_REV02.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdAllStations
    {
        // 🔥 1. เปลี่ยนจาก List<dynamic> เป็น List<cmlTCNM_MASTER_Stations>
        public static List<cmlTCNM_MASTER_Stations> C_PRCaAllStations()
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                // 🔥 2. ใช้ AS จับคู่ให้ตรงกับ Property ใน Model
                oSql.AppendLine($"SELECT FTCode AS tFTCode, FNStationId AS nFNStationId ");
                oSql.AppendLine($"FROM {cmlTable.tTCNM_MASTER_Stations};");

                // 🔥 3. เปลี่ยน Generic Type ตรงนี้
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