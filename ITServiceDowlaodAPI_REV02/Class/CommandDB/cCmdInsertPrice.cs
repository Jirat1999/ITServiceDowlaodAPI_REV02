using ITServiceDowlaodAPI_REV02.Models.Database;
using System.Text;
namespace ITServiceDowlaodAPI_REV02.Class.CommandDB
{
    public class cCmdInsertPrice
    {
        public static bool C_PRCbInsertPrice(int pnStationId, int pnFuelTypeId, string pdDate, decimal pcPrice)
        {
            StringBuilder? oSql = new();
            cDatabase? oDB = new();
            try
            {
                oSql.AppendLine($"INSERT INTO {cmlTable.tTCNM_PRICE_FuelPrices} (FNStationId, FNFuelTypeId, FDEffectiveDate, FCPrice)");
                oSql.AppendLine($"VALUES ({pnStationId}, {pnFuelTypeId}, '{pdDate}', {pcPrice});");
                return oDB.C_PRCbExecuteNoQuery(oSql.ToString(), cConfig.oC_ConfigDB);
            }
            catch (Exception oEx) { cConsole.C_PRCxLogError("cCmdInsertPrice: " + oEx.Message); return false; }
        }
    }
}