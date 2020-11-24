namespace FroniusDataShovel.SolarApi.GetPowerFlowRealtimeData {
    public record Status(
        int Code,
        string Reason,
        string UserMessage
    );
}