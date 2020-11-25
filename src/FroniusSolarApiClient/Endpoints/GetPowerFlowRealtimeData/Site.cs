using System.Text.Json.Serialization;

namespace FroniusSolarApiClient.Endpoints.GetPowerFlowRealtimeData {
    public record Site {
        [JsonPropertyName("E_Day")]
        double EnergyDay { get; init; }

        [JsonPropertyName("E_Year")]
        double EnergyYear { get; init; }

        [JsonPropertyName("E_Total")]
        double EnergyTotal { get; init; }

        [JsonPropertyName("Meter_Location")]
        string MeterLocation { get; init; }

        [JsonPropertyName("Mode")]
        string Mode { get; init; }

        [JsonPropertyName("P_Akku")]
        double AccumulatorPower { get; init; }

        [JsonPropertyName("P_Grid")]
        double GridPower { get; init; }

        [JsonPropertyName("P_Load")]
        double LoadPower { get; init; }

        [JsonPropertyName("P_PV")]
        double ArrayPower { get; init; }

        [JsonPropertyName("rel_Autonomy")]
        double AutonomyPercent { get; init; }

        [JsonPropertyName("rel_SelfConsumption")]
        double SelfConsumptionPercent { get; init; }
    }
}