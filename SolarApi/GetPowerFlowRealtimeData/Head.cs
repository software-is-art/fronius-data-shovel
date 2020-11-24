using System;
using System.Collections.Generic;

namespace FroniusDataShovel.SolarApi.GetPowerFlowRealtimeData {
    public record Head(
        Dictionary<string, string> RequestArguments,
        Status Status,
        DateTime Timestamp
    );
}