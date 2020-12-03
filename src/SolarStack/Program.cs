using Amazon.CDK;
using Env = System.Environment;

namespace SolarStack
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var region = Env.GetEnvironmentVariable("AWS_DEFAULT_REGION");
            if (string.IsNullOrEmpty(region))
            {
                throw new System.Exception("Environment variable AWS_DEFAULT_REGION is not set");
            }
            var accountId = Env.GetEnvironmentVariable("AWS_ACCOUNT_ID");
            if (string.IsNullOrEmpty(accountId))
            {
                throw new System.Exception("Environment variable AWS_ACCOUNT_ID is not set");
            }
            var app = new App();
            new SolarStack(app, new StackProps
            {
                Env = new Environment
                {
                    Account = accountId,
                    Region = region
                }
            });
            app.Synth();
        }
    }
}
