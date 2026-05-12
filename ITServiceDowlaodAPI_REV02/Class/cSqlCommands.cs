using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cSqlCommands
    {
        public static string C_PRCtGetStationId()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FNStationId FROM {cmlTable.tTCNM_MASTER_Stations} WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtGetStationId: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtGetStationId", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtInsertStation()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtInsertStation: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtInsertStation", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtUpdateStation()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_MASTER_Stations} SET FTName = @Name, FDUpdatedAt = GETDATE() WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtUpdateStation: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtUpdateStation", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtAllStations()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FTCode , FNStationId FROM {cmlTable.tTCNM_MASTER_Stations};");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("cSqlCommands.C_GETxAllStations: " + oEx.Message);
                cLog.C_WRTxLog("cSqlCommands", "C_GETxAllStations", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_GETxAllStations_toListVersion()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FTCode as tFTCode, FNStationId as nFNStationId FROM {cmlTable.tTCNM_MASTER_Stations};");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtAllStations: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtAllStations", oEx.Message);
                return string.Empty;
            }
        }

        // ==========================================
        // 2. TCNM_MASTER_FuelTypes (ชนิดน้ำมัน)
        // ==========================================
        public static string C_PRCtGetFuelTypeId()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FNFuelTypeId FROM {cmlTable.tTCNM_MASTER_FuelTypes} WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtGetFuelTypeId: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtGetFuelTypeId", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtInsertFuelType()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtInsertFuelType: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtInsertFuelType", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtUpdateFuelType()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_MASTER_FuelTypes} SET FTName = @Name, FDUpdatedAt = GETDATE() WHERE FTCode = @Code;");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtUpdateFuelType: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtUpdateFuelType", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtAllFuelTypes()
        {
            try
            {
                StringBuilder oSql = new StringBuilder();
                oSql.AppendLine($"SELECT FTCode, FNFuelTypeId FROM {cmlTable.tTCNM_MASTER_FuelTypes};");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtAllFuelTypes: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtAllFuelTypes", oEx.Message);
                return string.Empty;
            }
        }

        // ==========================================
        // 3. TCNM_PRICE_FuelPrices (ราคาน้ำมัน)
        // ==========================================
        public static string C_PRCtCheckLatestPrice()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtCheckLatestPrice: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtCheckLatestPrice", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtCheckPriceExistsForDate()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtCheckPriceExistsForDate: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtCheckPriceExistsForDate", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtInsertPrice()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtInsertPrice: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtInsertPrice", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtUpdatePrice()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtUpdatePrice: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtUpdatePrice", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtGetPricesByDate()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtGetPricesByDate: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtGetPricesByDate", oEx.Message);
                return string.Empty;
            }
        }

        // ==========================================
        // 4. Logs (บันทึกการทำงาน)
        // ==========================================
        public static string C_PRCtInsertLogStart()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtInsertLogStart: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtInsertLogStart", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtUpdateLogEnd()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtUpdateLogEnd: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtUpdateLogEnd", oEx.Message);
                return string.Empty;
            }
        }

        public static string C_PRCtInsertErrorLogs()
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
                cConsole.C_PRCxLogError("cSqlCommands.C_PRCtInsertErrorLogs: " + oEx.Message);
                cLog.C_PRCxLog("cSqlCommands", "C_PRCtInsertErrorLogs", oEx.Message);
                return string.Empty;
            }
        }
    }
}
