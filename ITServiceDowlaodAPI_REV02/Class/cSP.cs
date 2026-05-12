using ITServiceDowlaodAPI_REV02.Class.CommandDB;
using System.Globalization;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels; // อย่าลืม using Models สำหรับ cmlFuelPriceRoot

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cSP
    {
        // 1. โหลดข้อมูลสถานีบริการน้ำมัน (Stations)
        public Dictionary<string, int> C_GEToLoadStations()
        {
            Dictionary<string, int> oDbStations = new Dictionary<string, int>();
            var aAllStations = cCmdAllStations.C_PRCaAllStations();
            if (aAllStations != null && aAllStations.Count > 0)
            {
                foreach (var oItem in aAllStations)
                {
                    if (oItem != null && oItem.FTCode != null)
                    {
                        string tCode = ((string)oItem.FTCode).Trim().ToUpper();
                        int nId = (int)oItem.FNStationId;
                        if (!oDbStations.ContainsKey(tCode)) oDbStations.Add(tCode, nId);
                    }
                }
            }
            return oDbStations;
        }

        // 2. โหลดข้อมูลชนิดน้ำมัน (Fuel Types)
        public Dictionary<string, int> C_GEToLoadFuelTypes()
        {
            Dictionary<string, int> oDbFuelTypes = new Dictionary<string, int>();
            var aAllFuelTypes = cCmdAllFuelTypes.C_PRCaAllFuelTypes();
            if (aAllFuelTypes != null && aAllFuelTypes.Count > 0)
            {
                foreach (var oItem in aAllFuelTypes)
                {
                    if (oItem != null && oItem.FTCode != null)
                    {
                        string tCode = ((string)oItem.FTCode).Trim().ToUpper();
                        int nId = (int)oItem.FNFuelTypeId;
                        if (!oDbFuelTypes.ContainsKey(tCode)) oDbFuelTypes.Add(tCode, nId);
                    }
                }
            }
            return oDbFuelTypes;
        }

        // 3. โหลดข้อมูลราคาล่าสุดของวันนี้ (Prices)
        public Dictionary<string, decimal> C_GEToLoadPrices(string ptFormattedDate)
        {
            Dictionary<string, decimal> oDictPrices = new Dictionary<string, decimal>();
            var aAllPrices = cCmdGetPricesByDate.C_PRCaGetPricesByDate(ptFormattedDate);
            if (aAllPrices != null && aAllPrices.Count > 0)
            {
                foreach (var oItem in aAllPrices)
                {
                    if (oItem != null)
                    {
                        string tKey = $"{oItem.nFNStationId}_{oItem.nFNFuelTypeId}";
                        decimal cPrice = oItem.cFCPrice;

                        if (!oDictPrices.ContainsKey(tKey)) oDictPrices.Add(tKey, cPrice);
                        else oDictPrices[tKey] = cPrice;
                    }
                }
            }
            return oDictPrices;
        }

        // =========================================================
        // 🔥 4. เพิ่มฟังก์ชันแปลงวันที่ (ย้ายมาจาก Service)
        // =========================================================
        public string C_GETtFormattedDate(cmlFuelPriceRoot poData)
        {
            try
            {
                string tDateStr = poData?.poResponse?.tDate ?? DateTime.Now.ToString("yyyy-MM-dd");
                DateTime dEffDate = DateTime.TryParse(tDateStr, new CultureInfo("TH-th"), DateTimeStyles.None, out DateTime dPrase) ? dPrase : DateTime.Now.Date;
                return dEffDate.ToString("yyyy-MM-dd");
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("cSP.C_GETtFormattedDate: " + oEx.Message);
                return DateTime.Now.ToString("yyyy-MM-dd"); // ถ้าพังให้ใช้วันที่ปัจจุบันแทน
            }
        }
    }
}