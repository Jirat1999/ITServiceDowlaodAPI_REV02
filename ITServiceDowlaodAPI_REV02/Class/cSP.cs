using ITServiceDowlaodAPI_REV02.Class.CommandDB;
using System.Globalization;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cSP
    {
        public Dictionary<string, int> C_PRCoLoadStations()
        {
            Dictionary<string, int> oDbStations = new Dictionary<string, int>();
            try
            {
                var aAllStations = cCmdAllStations.C_PRCaAllStations();
                if (aAllStations != null && aAllStations.Count > 0)
                {
                    foreach (var oItem in aAllStations)
                    {
                        if (!string.IsNullOrEmpty(oItem.tFTCode))
                        {
                            string tCode = oItem.tFTCode.Trim().ToUpper();
                            int nId = oItem.nFNStationId;
                            if (!oDbStations.ContainsKey(tCode)) oDbStations.Add(tCode, nId);
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSP.C_PRCoLoadStations: " + oEx.Message);
            }
            return oDbStations;
        }

        public Dictionary<string, int> C_PRCaoLoadFuelTypes()
        {
            Dictionary<string, int> oDbFuelTypes = new Dictionary<string, int>();
            try
            {
                var oAllFuelTypes = cCmdAllFuelTypes.C_PRCaAllFuelTypes();
                if (oAllFuelTypes != null && oAllFuelTypes.Count > 0)
                {
                    foreach (var oItem in oAllFuelTypes)
                    {
                        if (!string.IsNullOrEmpty(oItem.tFTCode))
                        {
                            string tCode = oItem.tFTCode.Trim().ToUpper();
                            int nId = oItem.nFNFuelTypeId;

                            if (!oDbFuelTypes.ContainsKey(tCode)) oDbFuelTypes.Add(tCode, nId);
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSP.C_PRCaoLoadFuelTypes: " + oEx.Message);
            }
            return oDbFuelTypes;
        }

        public Dictionary<string, decimal> C_PRCaoLoadPrices(string ptFormattedDate)
        {
            Dictionary<string, decimal> aoDictPrices = new Dictionary<string, decimal>();
            try
            {
                var aAllPrices = cCmdGetPricesByDate.C_PRCaGetPricesByDate(ptFormattedDate);
                if (aAllPrices != null && aAllPrices.Count > 0)
                {
                    foreach (var oItem in aAllPrices)
                    {
                        if (oItem != null)
                        {
                            string tKey = $"{oItem.nFNStationId}_{oItem.nFNFuelTypeId}";
                            decimal cPrice = oItem.cFCPrice;

                            if (!aoDictPrices.ContainsKey(tKey)) aoDictPrices.Add(tKey, cPrice);
                            else aoDictPrices[tKey] = cPrice;
                        }
                    }
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSP.C_PRCaoLoadPrices: " + oEx.Message);
            }
            return aoDictPrices;
        }

        public string C_PRCtFormattedDate(cmlFuelPriceRoot poData)
        {
            try
            {
                string tDateStr = poData?.poResponse?.tDate ?? DateTime.Now.ToString("yyyy-MM-dd");
                DateTime dEffDate = DateTime.TryParse(tDateStr, new CultureInfo("TH-th"), DateTimeStyles.None, out DateTime dPrase) ? dPrase : DateTime.Now.Date;
                return dEffDate.ToString("yyyy-MM-dd");
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSP.C_PRCtFormattedDate: " + oEx.Message);
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
    }
}