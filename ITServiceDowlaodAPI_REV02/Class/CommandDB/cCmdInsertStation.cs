using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdInsertStation
    {
        public static int C_PRCnInsertStation(string ptCode, string ptName)
        {
            StringBuilder? oSql = new(); 
            cDatabase? oDB = new();
            try
            {
                string tSafeName = ptName?.Replace("'", "''") ?? "";
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_MASTER_Stations} (FTCode, FTName) VALUES ('{ptCode}', N'{ptName}');");
                oSql.AppendLine($"SELECT CAST(SCOPE_IDENTITY() as INT);");
                var aRes = oDB.C_PRCaoQuerytoListObj<int>(oSql.ToString(), cConfig.oC_ConfigDB);
                return (aRes != null && aRes.Count > 0) ? aRes[0] : 0;
            }
            catch (Exception oEx) 
            { 
                cConsole.C_PRCxLogError("cCmdInsertStation: " + oEx.Message); 
                return 0; 
            }
        }
    }
}