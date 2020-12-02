using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
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
            : base(scope, AWSConstructs.Names.Stack, props)
        {
            var queue = new Queue(this, nameof(AWSConstructs.Names.FroniusIngressQueue), new QueueProps {
                Fifo = true,
                Encryption = QueueEncryption.KMS_MANAGED,
                ContentBasedDeduplication = true,
                QueueName = AWSConstructs.Names.FroniusIngressQueue
            });

            var function = new Function(this, nameof(AWSConstructs.Names.FroniusIngressHandler), new FunctionProps {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/LambdaHandlers/bin/Release/netcoreapp3.1/LambdaHandlers.zip"),
                Handler = LambdaHandlers.FroniusRealtimeHandler.HandlerName,
                FunctionName = AWSConstructs.Names.FroniusIngressHandler
            });
            function.AddEventSource(new SqsEventSource(queue));
        }
    }
}
