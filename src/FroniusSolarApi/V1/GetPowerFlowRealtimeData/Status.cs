namespace FroniusSolarApi.V1.GetPowerFlowRealtimeData {
    public record Status(
        int Code,
        string Reason,
        string UserMessage
    );
}