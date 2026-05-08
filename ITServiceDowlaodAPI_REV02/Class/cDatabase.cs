using Dapper;
using ITServiceDowlaodAPI_REV02.Models.Setting;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDatabase
    {
        public string C_CONtDatabase(cmlConfigDB poConfig)
        {
            if (poConfig == null || string.IsNullOrEmpty(poConfig.tServerDB) || string.IsNullOrEmpty(poConfig.tNameDB)) return string.Empty;
            if (poConfig.bIntegratedSecurity) return $"Server={poConfig.tServerDB}; Database={poConfig.tNameDB}; TrustServerCertificate=true; Integrated Security=true; Max Pool Size=100";
            return $"Server={poConfig.tServerDB}; Database={poConfig.tNameDB}; User Id={poConfig.tUser}; Password={poConfig.tPassword}; TrustServerCertificate=true; Max Pool Size=100";
        }

        public bool C_PRCbExecuteNoQuery(string ptSqlCmd, cmlConfigDB poConnStr, object poParam = null, int pnCmdTimeout = 60)
        {
            try
            {
                using var oSqlConn = new SqlConnection(C_CONtDatabase(poConnStr));
                oSqlConn.Open();
                return oSqlConn.Execute(ptSqlCmd, poParam, commandTimeout: pnCmdTimeout) > 0;
            }
            catch (Exception oEx) { cConsole.C_LogError(oEx.Message); }
            return false;
        }

        public List<T> C_GETaQuerytoListObj<T>(string ptSqlCmd, cmlConfigDB poConfig, object poParam = null, int pnCmdTimeout = 60)
        {
            try
            {
                using SqlConnection oDbCon = new SqlConnection(C_CONtDatabase(poConfig));
                oDbCon.Open();
                return oDbCon.Query<T>(ptSqlCmd, poParam, commandTimeout: pnCmdTimeout).ToList();
            }
            catch (Exception oEx) { cConsole.C_LogError(oEx.Message); }
            return new List<T>();
        }
    }
}
