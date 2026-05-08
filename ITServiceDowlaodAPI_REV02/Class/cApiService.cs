using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static ITServiceDowlaodAPI_REV02.Models.cmlFuelPriceModels;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cApiService
    {
        private static readonly HttpClient oClient = new HttpClient();

        public static async Task<(cmlFuelPriceRoot? oData, string tRawJson)> C_GETxOilPriceAsync(CancellationToken poCt)
        {
            string tUrl = cConfig.oApiConfig?.tUrl ?? string.Empty;
            if (string.IsNullOrEmpty(tUrl)) return (null, string.Empty);

            try
            {
                var oResponse = await oClient.GetAsync(tUrl, poCt);
                oResponse.EnsureSuccessStatusCode();

                string tJsonString = await oResponse.Content.ReadAsStringAsync(poCt);
                var oFuelRoot = JsonSerializer.Deserialize<cmlFuelPriceRoot>(tJsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (oFuelRoot?.tStatus == "success" && oFuelRoot.poResponse != null)
                {
                    cConsole.C_LogProcess($">>> Download successfully. Date from API: {oFuelRoot.poResponse.tDate}");
                    return (oFuelRoot, tJsonString);
                }
                return (null, string.Empty);
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError($">>> Error fetching API: {oEx.Message}");
                cLog.C_WRTxLog("cApiService", "C_GETxOilPriceAsync", oEx.Message);
                return (null, string.Empty);
            }
        }
    }
}
