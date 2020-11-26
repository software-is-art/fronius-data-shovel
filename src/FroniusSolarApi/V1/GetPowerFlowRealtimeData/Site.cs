using System.Text.Json.Serialization;

namespace FroniusSolarApi.V1.GetPowerFlowRealtimeData {
    public record Site {
        [JsonPropertyName("E_Day")]
        public double? EnergyDay { get; init; }

        [JsonPropertyName("E_Year")]
        public double? EnergyYear { get; init; }

        [JsonPropertyName("E_Total")]
        public double? EnergyTotal { get; init; }

        [JsonPropertyName("Meter_Location")]
        public string MeterLocation { get; init; }

        [JsonPropertyName("Mode")]
        public string Mode { get; init; }

        [JsonPropertyName("P_Akku")]
        public double? AccumulatorPower { get; init; }

        [JsonPropertyName("P_Grid")]
        public double? GridPower { get; init; }

        [JsonPropertyName("P_Load")]
        public double? LoadPower { get; init; }

        [JsonPropertyName("P_PV")]
        public double? ArrayPower { get; init; }

        [JsonPropertyName("rel_Autonomy")]
        public double? AutonomyPercent { get; init; }

        [JsonPropertyName("rel_SelfConsumption")]
        public double? SelfConsumptionPercent { get; init; }
    }
}