using System;

namespace AWSConstructs
{
    public static class Names
    {
        public static string Stack { get; } = $"SolarStack-{ThisAssembly.Git.Branch.Replace("/", "-")}";
        public static string FroniusIngressQueue { get; } = $"{Stack}-{nameof(FroniusIngressQueue)}";
        public static string FroniusIngressHandler { get; } = $"{Stack}-{nameof(FroniusIngressHandler)}";
        public static string RealtimeDataAggregatorHandler { get; } = $"{Stack}-{nameof(RealtimeDataAggregatorHandler)}";
        public static string RealtimeDataTable { get; } = $"{Stack}-{nameof(RealtimeDataTable)}";
        public static string RealtimeDataTablePartitionKey { get; } = "TimeBucket";
        public static string RealtimeDataTableSortKey { get; } = "Timestamp";
    }
}
