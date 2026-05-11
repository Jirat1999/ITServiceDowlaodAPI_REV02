using System.Text.Json;
using System.Text.Json.Serialization;

namespace ITServiceDowlaodAPI_REV02.Models
{
    public class cmlFuelPriceModels
    {
        public class cmlFuelPriceRoot
        {
            [JsonPropertyName("status")] public string? tStatus { get; set; }
            [JsonPropertyName("response")] public cmlFuelData? poResponse { get; set; }
            [JsonIgnore] public string? tRawJson { get; set; }
        }

        public class cmlFuelData
        {
            [JsonPropertyName("date")] public string? tDate { get; set; }
            [JsonPropertyName("stations")] public Dictionary<string, Dictionary<string, cmlFuelType>>? tStations { get; set; }
        }

        public class cmlFuelType
        {
            [JsonPropertyName("name")] public string? tName { get; set; }
            [JsonPropertyName("price")] public JsonElement oRawPrice { get; set; }
            [JsonIgnore]
            public decimal cNumericPrice
            {
                get
                {
                    if (oRawPrice.ValueKind == JsonValueKind.Number) return oRawPrice.GetDecimal();
                    if (oRawPrice.ValueKind == JsonValueKind.String && decimal.TryParse(oRawPrice.GetString(), out decimal nParsed)) return nParsed;
                    return 0m;
                }
            }
        }
    }
}
