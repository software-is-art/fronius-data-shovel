using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.SQS;
using Amazon.CDK.AWS.DynamoDB;

namespace SolarStack
{
    public class SolarStack : Stack
    {
        internal SolarStack(Construct scope, IStackProps props = null)
            : base(scope, AWSConstructs.Names.Stack, props)
        {
            const string lambdaSource = "src/LambdaHandlers/bin/release/netcoreapp3.1/LambdaHandlers.zip";
            var ingressQueue = new Queue(this, nameof(AWSConstructs.Names.FroniusIngressQueue), new QueueProps
            {
                Encryption = QueueEncryption.KMS_MANAGED,
                QueueName = AWSConstructs.Names.FroniusIngressQueue,
                VisibilityTimeout = Duration.Seconds(43200)
            });

            var realtimeDataTable = new Table(this, AWSConstructs.Names.RealtimeDataTable, new TableProps
            {
                PartitionKey = new Attribute { Name = AWSConstructs.Names.RealtimeDataTablePartitionKey, Type = AttributeType.STRING },
                SortKey = new Attribute { Name = AWSConstructs.Names.RealtimeDataTableSortKey, Type = AttributeType.NUMBER },
                BillingMode = BillingMode.PAY_PER_REQUEST,
                TableName = AWSConstructs.Names.RealtimeDataTable,
                Stream = StreamViewType.NEW_IMAGE
            });

            var ingressFunction = new Function(this, nameof(AWSConstructs.Names.FroniusIngressHandler),
            new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset(lambdaSource),
                Handler = LambdaHandlers.FroniusRealtimeHandler.HandlerName,
                FunctionName = AWSConstructs.Names.FroniusIngressHandler,
                Timeout = Duration.Minutes(5)
            });

            ingressFunction.AddEventSource(new SqsEventSource(ingressQueue));
            _ = realtimeDataTable.GrantReadWriteData(ingressFunction);
            _ = realtimeDataTable.Grant(ingressFunction, "dynamodb:DescribeTable");

            var aggregateFunction = new Function(this, nameof(AWSConstructs.Names.RealtimeDataAggregatorHandler),
            new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset(lambdaSource),
                Handler = LambdaHandlers.RealtimeDataAggregatorHandler.HandlerName,
                FunctionName = AWSConstructs.Names.RealtimeDataAggregatorHandler,
                Timeout = Duration.Minutes(5)
            });

            aggregateFunction.AddEventSource(new DynamoEventSource(realtimeDataTable,
            new DynamoEventSourceProps
            {
                StartingPosition = StartingPosition.LATEST,
                ParallelizationFactor = 10,
                BatchSize = 1
            }));
            _ = realtimeDataTable.GrantReadWriteData(aggregateFunction);
            _ = realtimeDataTable.Grant(aggregateFunction, "dynamodb:DescribeTable");
        }
    }
}
