using CommandLine;

namespace FroniusDataShovel {
	public class Options {
		[Option("aws-account-id", Required = true, HelpText = "AWS Account Id to connect to")]
		public string? AwsAccountId { get; set; }
	}
}
