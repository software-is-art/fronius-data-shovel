namespace FroniusSolarApiClient.Endpoints.GetPowerFlowRealtimeData {
    public record Status(
        int Code,
        string Reason,
        string UserMessage
    );
}