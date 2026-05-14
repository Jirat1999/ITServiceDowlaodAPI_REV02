using Dapper;
using ITServiceDowlaodAPI_REV02.Models.Setting;
using Microsoft.Data.SqlClient;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cDatabase
    {
        public string C_PRCtDatabase(cmlConfigDB poConfig)
        {
            try
            {
                if (poConfig == null || string.IsNullOrEmpty(poConfig.tServerDB) || string.IsNullOrEmpty(poConfig.tNameDB)) return string.Empty;

                if (poConfig.bIntegratedSecurity)
                {
                    return $"Server={poConfig.tServerDB}; Database={poConfig.tNameDB}; TrustServerCertificate=true; Integrated Security=true; Max Pool Size=100";
                }

                return $"Server={poConfig.tServerDB}; Database={poConfig.tNameDB}; User Id={poConfig.tUser}; Password={poConfig.tPassword}; TrustServerCertificate=true; Max Pool Size=100";
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cDatabase.C_PRCtDatabase: " + oEx.Message);
                return string.Empty;
            }
        }

        public bool C_PRCbExecuteNoQuery(string ptSqlCmd, cmlConfigDB poConnStr, object? poParam = null, int pnCmdTimeout = 60)
        {
            try
            {
                using var oSqlConn = new SqlConnection(C_PRCtDatabase(poConnStr));
                oSqlConn.Open();
                return oSqlConn.Execute(ptSqlCmd, poParam, commandTimeout: pnCmdTimeout) > 0;
            }
            catch (Exception oEx) 
            { 
                cConsole.C_PRCxLogError(oEx.Message); 
            }
            return false;
        }

        public List<T> C_PRCaoQuerytoListObj<T>(string ptSqlCmd, cmlConfigDB poConfig, object? poParam = null, int pnCmdTimeout = 60)
        {
            try
            {
                using SqlConnection oDbCon = new SqlConnection(C_PRCtDatabase(poConfig));
                oDbCon.Open();
                return oDbCon.Query<T>(ptSqlCmd, poParam, commandTimeout: pnCmdTimeout).ToList();
            }
            catch (Exception oEx) 
            { 
                cConsole.C_PRCxLogError(oEx.Message); 
            }
            return new List<T>();
        }
    }
}
