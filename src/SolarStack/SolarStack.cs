using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.SQS;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Collections.Generic;

namespace SolarStack
{
    public class SolarStack : Stack
    {
        internal SolarStack(Construct scope, IStackProps props = null) 
            : base(scope, $"SolarStack-{ThisAssembly.Git.Branch}", props)
        {
            var queue = new Queue(this, "FroniusRealtimeIngressQueue", new QueueProps {
                Fifo = true,
                Encryption = QueueEncryption.KMS_MANAGED,
                QueueName = "FroniusRealtimeIngressQueue.fifo"
            });

            var function = new Function(this, "FroniusRealtimeIngressHandler", new FunctionProps {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/LambdaHandlers/bin/Release/netcoreapp3.1/LambdaHandlers.zip"),
                Handler = "LambdaHandlers::LambdaHandlers.FroniusRealtimeHandler::FunctionHandler",
            });
            function.AddEventSource(new SqsEventSource(queue));
        }
    }
}
