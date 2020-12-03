using System.Text.Json.Serialization;

namespace Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData {
	public class Inverter {
        [JsonPropertyName("DT")]
        public int DeviceType { get; set; }

        [JsonPropertyName("E_Day")]
        public double? EnergyDay { get; set; }

        [JsonPropertyName("E_Year")]
        public double? EnergyYear { get; set; }

        [JsonPropertyName("E_Total")]
        public double? EnergyTotal { get; set; }

        [JsonPropertyName("P")]
        public double? Power { get; set; }
    }
}
