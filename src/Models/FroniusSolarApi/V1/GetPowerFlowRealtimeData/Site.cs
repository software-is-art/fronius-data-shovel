using System.Text.Json.Serialization;

namespace Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData
{
    public class Site
    {
        [JsonPropertyName("E_Day")]
        public double? EnergyDay { get; set; }

        [JsonPropertyName("E_Year")]
        public double? EnergyYear { get; set; }

        [JsonPropertyName("E_Total")]
        public double? EnergyTotal { get; set; }

        [JsonPropertyName("Meter_Location")]
        public string MeterLocation { get; set; }

        [JsonPropertyName("Mode")]
        public string Mode { get; set; }

        [JsonPropertyName("P_Akku")]
        public double? AccumulatorPower { get; set; }

        [JsonPropertyName("P_Grid")]
        public double? GridPower { get; set; }

        [JsonPropertyName("P_Load")]
        public double? LoadPower { get; set; }

        [JsonPropertyName("P_PV")]
        public double? ArrayPower { get; set; }

        [JsonPropertyName("rel_Autonomy")]
        public double? AutonomyPercent { get; set; }

        [JsonPropertyName("rel_SelfConsumption")]
        public double? SelfConsumptionPercent { get; set; }
    }
}
