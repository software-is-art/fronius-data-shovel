using System.Collections.Generic;

namespace FroniusSolarApi.V1.GetPowerFlowRealtimeData {
    public record Data(
        Dictionary<string, Inverter> Inverters,
        Site Site,
        string Version
    );
}