using System.Text.Json;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cApiService
    {
        private static readonly HttpClient oClient = new HttpClient();

        public static async Task<(cmlFuelPriceRoot? oData, string tRawJson)> C_PRCtOilPriceAsync(CancellationToken poCt)
        {
            string tUrl = cConfig.oC_ApiConfig?.tUrl ?? string.Empty;
            if (string.IsNullOrEmpty(tUrl)) return (null, string.Empty);

            try
            {
                var oResponse = await oClient.GetAsync(tUrl, poCt);
                oResponse.EnsureSuccessStatusCode();

                string tJsonString = await oResponse.Content.ReadAsStringAsync(poCt);
                var oFuelRoot = JsonSerializer.Deserialize<cmlFuelPriceRoot>(tJsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (oFuelRoot?.tStatus == "success" && oFuelRoot.poResponse != null)
                {
                    cConsole.C_PRCxLogProcess($">>> Download successfully. Date from API: {oFuelRoot.poResponse.tDate}");
                    return (oFuelRoot, tJsonString);
                }
                return (null, string.Empty);
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError($">>> Error fetching API: {oEx.Message}");
                cLog.C_PRCxLog("cApiService", "C_GETxOilPriceAsync", oEx.Message);
                return (null, string.Empty);
            }
        }
    }
}
