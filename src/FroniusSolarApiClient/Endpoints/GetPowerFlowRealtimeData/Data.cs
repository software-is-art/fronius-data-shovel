using System.Collections.Generic;

namespace FroniusSolarApiClient.Endpoints.GetPowerFlowRealtimeData {
    public record Data(
        Dictionary<string, Inverter> Inverters,
        Site Site,
        string Version
    );
}