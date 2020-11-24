using System.Collections.Generic;

namespace FroniusDataShovel.SolarApi.GetPowerFlowRealtimeData {
    public record Data(
        Dictionary<string, Inverter> Inverters,
        Site Site,
        string Version
    );
}