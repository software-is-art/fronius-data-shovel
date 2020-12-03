using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.SQS;
using Amazon.CDK.AWS.DynamoDB;

namespace SolarStack {
    public class SolarStack : Stack
    {
        internal SolarStack(Construct scope, IStackProps props = null) 
            : base(scope, AWSConstructs.Names.Stack, props)
        {
            var ingressQueue = new Queue(this, nameof(AWSConstructs.Names.FroniusIngressQueue), new QueueProps {
                Fifo = true,
                Encryption = QueueEncryption.KMS_MANAGED,
                ContentBasedDeduplication = true,
                QueueName = AWSConstructs.Names.FroniusIngressQueue
            });

            var realtimeDataTable = new Table(this, AWSConstructs.Names.RealtimeDataTable, new TableProps {
                PartitionKey = new Attribute { Name = AWSConstructs.Names.RealtimeDataTablePartitionKey, Type = AttributeType.STRING },
                SortKey = new Attribute { Name = AWSConstructs.Names.RealtimeDataTableSortKey, Type = AttributeType.NUMBER },
                BillingMode = BillingMode.PAY_PER_REQUEST,
                TableName = AWSConstructs.Names.RealtimeDataTable
            });

            var ingressFunction = new Function(this, nameof(AWSConstructs.Names.FroniusIngressHandler), new FunctionProps {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/LambdaHandlers/bin/Release/netcoreapp3.1/LambdaHandlers.zip"),
                Handler = LambdaHandlers.FroniusRealtimeHandler.HandlerName,
                FunctionName = AWSConstructs.Names.FroniusIngressHandler,
                Timeout = Duration.Minutes(5)
            });

            ingressFunction.AddEventSource(new SqsEventSource(ingressQueue));

			_ = realtimeDataTable.GrantReadWriteData(ingressFunction);
			_ = realtimeDataTable.Grant(ingressFunction, "dynamodb:DescribeTable");
        }
    }
}
