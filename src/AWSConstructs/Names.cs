using System;

namespace AWSConstructs {
	public static class Names {
		public static string Stack { get; } = $"SolarStack-{ThisAssembly.Git.Branch}";
		public static string FroniusIngressQueue { get; } = $"{Stack}-{nameof(FroniusIngressQueue)}.fifo";
		public static string FroniusIngressHandler { get; } = $"{Stack}-{nameof(FroniusIngressHandler)}";
	}
}
