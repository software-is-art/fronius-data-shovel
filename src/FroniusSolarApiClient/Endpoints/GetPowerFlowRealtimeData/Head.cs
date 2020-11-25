using System;
using System.Collections.Generic;

namespace FroniusSolarApiClient.Endpoints.GetPowerFlowRealtimeData {
    public record Head(
        Dictionary<string, string> RequestArguments,
        Status Status,
        DateTime Timestamp
    );
}