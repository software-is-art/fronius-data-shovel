using System.Text.Json.Serialization;

namespace Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData
{
    public class Inverter
    {
        [JsonPropertyName("DT")]
        public int DeviceType { get; set; }

        [JsonPropertyName("E_Day")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? EnergyDay { get; set; }

        [JsonPropertyName("E_Year")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? EnergyYear { get; set; }

        [JsonPropertyName("E_Total")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? EnergyTotal { get; set; }

        [JsonPropertyName("P")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? Power { get; set; }
    }
}
