using System;
using System.Collections.Generic;

namespace FroniusSolarApi.V1.GetPowerFlowRealtimeData {
    public record Head(
        Dictionary<string, string> RequestArguments,
        Status Status,
        DateTime Timestamp
    );
}