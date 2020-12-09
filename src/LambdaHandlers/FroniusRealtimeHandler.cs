using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaHandlers
{
    public class FroniusRealtimeHandler
    {
        public static string HandlerName { get; } = $"{nameof(LambdaHandlers)}::{nameof(LambdaHandlers)}.{nameof(FroniusRealtimeHandler)}::{nameof(FunctionHandler)}";

        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            var client = new AmazonDynamoDBClient(
                new AmazonDynamoDBConfig
                {
                    RegionEndpoint = RegionEndpoint.USEast1
                }
            );
            var table = Table.LoadTable(client, AWSConstructs.Names.RealtimeDataTable);

            // TO DO - batch insert to Dynamo
            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context, table);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context, Table table)
        {
            var response = JsonSerializer.Deserialize<Response>(message.Body);
            var site = response.Body.Data.Site;
            var timestamp = response.Head.Timestamp;
            var document = new Document
            {
                [AWSConstructs.Names.RealtimeDataTablePartitionKey] = timestamp.ToString("yyyy-MM-dd-HH-mm"),
                [AWSConstructs.Names.RealtimeDataTableSortKey] = ((DateTimeOffset)timestamp).ToUnixTimeSeconds(),
                ["EnergyDay"] = site.EnergyDay,
                ["EnergyYear"] = site.EnergyYear,
                ["EnergyTotal"] = site.EnergyTotal,
                ["AccumulatorPower"] = site.AccumulatorPower,
                ["GridPower"] = site.GridPower,
                ["LoadPower"] = site.LoadPower,
                ["CollectorPower"] = site.ArrayPower,
                ["Autonomy"] = site.AutonomyPercent,
                ["SelfConsumption"] = site.SelfConsumptionPercent
            };
            _ = await table.PutItemAsync(document);
        }
    }
}
