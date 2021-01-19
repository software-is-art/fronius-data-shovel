using System;
using System.IO;
using System.Text;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.Lambda.Serialization.Json;
using Amazon.DynamoDBv2.Model;

namespace LambdaHandlers
{
    public class RealtimeDataAggregatorHandler
    {
        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public static string HandlerName { get; } = $"{nameof(LambdaHandlers)}::{nameof(LambdaHandlers)}.{nameof(RealtimeDataAggregatorHandler)}::{nameof(FunctionHandler)}";

        public void FunctionHandler(DynamoDBEvent dynamoEvent)
        {
            foreach (var record in dynamoEvent.Records)
            {
                // TODO: Write DynamoDB update expression for each field to aggregate timeseries data
                var image = record.Dynamodb.NewImage;
                var timestamp = long.Parse(image["Timestamp"].N);
                var timebucket = image["TimeBucket"];
                var energyDay = GetDouble(image["EnergyDay"]); // Use MAX
                var gridPower = GetDouble(image["GridPower"]); // Use MIN, MAX, AVERAGE, and SUM
                var selfConsumption = GetDouble(image["SelfConsumption"]); // Use MAX and SUM
                var accumulatorPower = GetDouble(image["AccumulatorPower"]); // Use MIN, MAX, AVERAGE, and SUM
                var energyYear = GetDouble(image["EnergyYear"]); // Use MAX
                var energyTotal = GetDouble(image["EnergyTotal"]); // Use MAX
                var arrayPower = GetDouble(image["ArrayPower"]); // Use MIN, MAX, AVERAGE, and SUM
                var autonomy = GetDouble(image["Autonomy"]); // Use MIN, MAX, AVERAGE, and SUM
                var loadPower = GetDouble(image["LoadPower"]); // Use MIN, MAX, AVERAGE, and SUM
                Console.WriteLine($@"Parsed:
Timestamp: {timestamp},
TimeBucket: {timebucket},
EnergyDay: {energyDay},
GridPower: {gridPower},
SelfConsumption: {selfConsumption},
AccumulatorPower: {accumulatorPower},
EnergyYear: {energyYear},
EnergyTotal: {energyTotal},
ArrayPower: {arrayPower},
Autonomy: {autonomy},
LoadPower: {loadPower} 
                ");
            }
        }

        private double? GetDouble(AttributeValue value)
        {
            if (value.NULL)
            {
                return null;
            }
            if (string.IsNullOrEmpty(value.S))
            {
                return double.Parse(value.N);
            }
            return double.NaN;
        }

        private string SerializeObject(object streamRecord)
        {
            using (var ms = new MemoryStream())
            {
                _jsonSerializer.Serialize(streamRecord, ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}