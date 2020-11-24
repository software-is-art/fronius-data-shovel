using System.Text.Json.Serialization;

namespace FroniusDataShovel.SolarApi.GetPowerFlowRealtimeData {
    public record Inverter {
        [JsonPropertyName("DT")]
        int DeviceType { get; init; }

        [JsonPropertyName("E_Day")]
        double? EnergyDay { get; init; }

        [JsonPropertyName("E_Year")]
        double? EnergyYear { get; init; }

        [JsonPropertyName("E_Total")]
        double? EnergyTotal { get; init; }

        [JsonPropertyName("P")]
        double? Power { get; init; }
    }
}