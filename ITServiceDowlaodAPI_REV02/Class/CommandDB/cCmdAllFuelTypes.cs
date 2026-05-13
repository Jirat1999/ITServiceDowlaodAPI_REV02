using ITServiceDowlaodAPI_REV02.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdAllFuelTypes
    {
        public static List<cmlTCNM_MASTER_FuelTypes> C_PRCaAllFuelTypes()
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"SELECT FTCode AS tFTCode, FNFuelTypeId AS nFNFuelTypeId ");
                oSql.AppendLine($"FROM {cmlTable.tTCNM_MASTER_FuelTypes};");

                return oDB.C_PRCaoQuerytoListObj<cmlTCNM_MASTER_FuelTypes>(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cCmdAllFuelTypes: " + oEx.Message);
                return new List<cmlTCNM_MASTER_FuelTypes>();
            }
        }
    }
}