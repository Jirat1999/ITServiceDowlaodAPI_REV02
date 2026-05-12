using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdGetPricesByDate
    {
        public static List<cmlTCNM_PRICE_FuelPrices> C_PRCaGetPricesByDate(string pdDate)
        {
            StringBuilder? oSql = new(); 
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"SELECT FNStationId AS nFNStationId, FNFuelTypeId AS nFNFuelTypeId, FCPrice AS cFCPrice");
                oSql.AppendLine($"FROM {cmlTable.tTCNM_PRICE_FuelPrices} WHERE CAST(FDEffectiveDate AS DATE) = CAST('{pdDate}' AS DATE);");
                return oDB.C_PRCaoQuerytoListObj<cmlTCNM_PRICE_FuelPrices>(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) { cConsole.C_PRCxLogError("cCmdGetPricesByDate: " + oEx.Message); return new List<cmlTCNM_PRICE_FuelPrices>(); }
        }
    }
}