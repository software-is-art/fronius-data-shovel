using System.Collections.Generic;

namespace Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData
{
    public class Data
    {
        public Dictionary<string, Inverter> Inverters { get; set; }
        public Site Site { get; set; }
        public string Version { get; set; }
    }
}
