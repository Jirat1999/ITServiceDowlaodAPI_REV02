using ITServiceDowlaodAPI_REV02.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cSqlCommands
    {
        public static string C_GETxGetStationId()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FNStationId FROM {cmlTable.tTCNM_MASTER_Stations} WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxGetStationId: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxGetStationId", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxInsertStation()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_MASTER_Stations} (FTCode, FTName) VALUES (@Code, @Name);");
                oSql.AppendLine($"SELECT CAST(SCOPE_IDENTITY() as INT);");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxInsertStation: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxInsertStation", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxUpdateStation()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_MASTER_Stations} SET FTName = @Name, FDUpdatedAt = GETDATE() WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxUpdateStation: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxUpdateStation", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxAllStations()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FTCode, FNStationId FROM {cmlTable.tTCNM_MASTER_Stations};");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxAllStations: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxAllStations", oEx.Message);
                return string.Empty;
            }
        }

        // ==========================================
        // 2. TCNM_MASTER_FuelTypes (ชนิดน้ำมัน)
        // ==========================================
        public static string C_GETxGetFuelTypeId()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FNFuelTypeId FROM {cmlTable.tTCNM_MASTER_FuelTypes} WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxGetFuelTypeId: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxGetFuelTypeId", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxInsertFuelType()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_MASTER_FuelTypes} (FTCode, FTName) VALUES (@Code, @Name);");
                oSql.AppendLine($"SELECT CAST(SCOPE_IDENTITY() as INT);");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxInsertFuelType: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxInsertFuelType", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxUpdateFuelType()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_MASTER_FuelTypes} SET FTName = @Name, FDUpdatedAt = GETDATE() WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxUpdateFuelType: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxUpdateFuelType", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxAllFuelTypes()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FTCode, FNFuelTypeId FROM {cmlTable.tTCNM_MASTER_FuelTypes};");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxAllFuelTypes: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxAllFuelTypes", oEx.Message);
                return string.Empty;
            }
        }

        // ==========================================
        // 3. TCNM_PRICE_FuelPrices (ราคาน้ำมัน)
        // ==========================================
        public static string C_GETxCheckLatestPrice()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT TOP 1");
                oSql.AppendLine($"P.FCPrice AS cFCPrice,");
                oSql.AppendLine($"P.FDEffectiveDate AS dFDEffectiveDate");
                oSql.AppendLine($"FROM {cmlTable.tTCNM_PRICE_FuelPrices} P");
                oSql.AppendLine($"INNER JOIN {cmlTable.tTCNM_MASTER_FuelTypes} F ON P.FNFuelTypeId = F.FNFuelTypeId");
                oSql.AppendLine($"WHERE P.FNStationId = @StationId");
                oSql.AppendLine($"AND F.FTCode = @FuelCode");
                oSql.AppendLine($"ORDER BY P.FDEffectiveDate DESC;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxCheckLatestPrice: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxCheckLatestPrice", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxCheckPriceExistsForDate()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT 1 FROM {cmlTable.tTCNM_PRICE_FuelPrices}");
                oSql.AppendLine($"WHERE FNStationId = @StationId AND FNFuelTypeId = @FuelTypeId AND FDEffectiveDate = @Date;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxCheckPriceExistsForDate: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxCheckPriceExistsForDate", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxInsertPrice()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_PRICE_FuelPrices} (FNStationId, FNFuelTypeId, FDEffectiveDate, FCPrice)");
                oSql.AppendLine($"VALUES (@StationId, @FuelTypeId, @Date, @Price);");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxInsertPrice: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxInsertPrice", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxUpdatePrice()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_PRICE_FuelPrices} SET FCPrice = @Price");
                oSql.AppendLine($"WHERE FNStationId = @StationId AND FNFuelTypeId = @FuelTypeId AND FDEffectiveDate = @Date;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxUpdatePrice: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxUpdatePrice", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxPricesByDate()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FNStationId AS nFNStationId, FNFuelTypeId AS nFNFuelTypeId, FCPrice AS cFCPrice");
                oSql.AppendLine($"FROM {cmlTable.tTCNM_PRICE_FuelPrices}");
                oSql.AppendLine($"WHERE CAST(FDEffectiveDate AS DATE) = CAST(@Date AS DATE);");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxPricesByDate: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxPricesByDate", oEx.Message);
                return string.Empty;
            }
        }

        // ==========================================
        // 4. Logs (บันทึกการทำงาน)
        // ==========================================
        public static string C_GETxInsertLogStart()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_LOG_FuelUpdate} (FDUpdateStart, FTStatus, FTPriceDataJSON)");
                oSql.AppendLine($"VALUES (GETDATE(), 'Processing', @Json);");
                oSql.AppendLine($"SELECT CAST(SCOPE_IDENTITY() as BIGINT);");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxInsertLogStart: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxInsertLogStart", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxUpdateLogEnd()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_LOG_FuelUpdate}");
                oSql.AppendLine($"SET FDUpdateEnd = GETDATE(), FNStationCount = @StaCount, FNPriceCount = @PriceCount, FTStatus = 'Success', FTMessage = 'Complete'");
                oSql.AppendLine($"WHERE FNLogId = @LogId;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxUpdateLogEnd: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxUpdateLogEnd", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxInsertErrorLogs()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_ERROR_ErrorLogs} (FTProcessName, FTErrorMessage, FTStackTrace)");
                oSql.AppendLine($"VALUES (@Proc, @Msg, @Stack);");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxInsertErrorLogs: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxInsertErrorLogs", oEx.Message);
                return string.Empty;
            }
        }
    }
}
