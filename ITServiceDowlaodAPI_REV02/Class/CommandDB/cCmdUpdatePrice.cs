using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdUpdatePrice
    {
        public static bool C_PRCbUpdatePrice(int pnStationId, int pnFuelTypeId, string pdDate, decimal pcPrice)
        {
            StringBuilder? oSql = new(); 
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"UPDATE {cmlTable.tTCNM_PRICE_FuelPrices} SET FCPrice = {pcPrice}");
                oSql.AppendLine($"WHERE FNStationId = {pnStationId} AND FNFuelTypeId = {pnFuelTypeId} AND FDEffectiveDate = '{pdDate}';");
                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) { cConsole.C_PRCxLogError("cCmdUpdatePrice: " + oEx.Message); return false; }
        }
    }
}