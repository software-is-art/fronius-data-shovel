using System.Text.Json.Serialization;

namespace FroniusSolarApi.V1.GetPowerFlowRealtimeData {
    public record Inverter {
        [JsonPropertyName("DT")]
        public int DeviceType { get; init; }

        [JsonPropertyName("E_Day")]
        public double? EnergyDay { get; init; }

        [JsonPropertyName("E_Year")]
        public double? EnergyYear { get; init; }

        [JsonPropertyName("E_Total")]
        public double? EnergyTotal { get; init; }

        [JsonPropertyName("P")]
        public double? Power { get; init; }
    }
}