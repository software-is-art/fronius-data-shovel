using System.Text.Json.Serialization;

namespace Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData
{
    public class Site
    {
        [JsonPropertyName("E_Day")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? EnergyDay { get; set; }

        [JsonPropertyName("E_Year")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? EnergyYear { get; set; }

        [JsonPropertyName("E_Total")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? EnergyTotal { get; set; }

        [JsonPropertyName("Meter_Location")]
        public string MeterLocation { get; set; }

        [JsonPropertyName("Mode")]
        public string Mode { get; set; }

        [JsonPropertyName("P_Akku")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? AccumulatorPower { get; set; }

        [JsonPropertyName("P_Grid")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? GridPower { get; set; }

        [JsonPropertyName("P_Load")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? LoadPower { get; set; }

        [JsonPropertyName("P_PV")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? ArrayPower { get; set; }

        [JsonPropertyName("rel_Autonomy")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? AutonomyPercent { get; set; }

        [JsonPropertyName("rel_SelfConsumption")]
        [JsonConverter(typeof(DoubleConverter))]
        public double? SelfConsumptionPercent { get; set; }
    }
}
