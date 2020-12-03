using System;
using System.Collections.Generic;

namespace Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData
{
    public class Head
    {
        public Dictionary<string, string> RequestArguments { get; set; }
        public Status Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
