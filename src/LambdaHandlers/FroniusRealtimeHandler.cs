using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaHandlers
{
    public class FroniusRealtimeHandler
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public FroniusRealtimeHandler()
        {

        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach(var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed message {message.Body}");

            // TODO: Do interesting work based on the new message
            await Task.CompletedTask;
        }

        public class Message {
            public Body Body { get; set; }
            public Head Head { get; set; }
        }
        public class Head {
            public Dictionary<string, string> RequestArguments { get; set; }
            public Status Status { get; set; }
            public DateTime TimeStamp { get; set; }
        }
        public class Status {
            public int Code { get; set; }
            public string Reason { get; set; }
            public string UserMessage { get; set; }
        }
        public class Body {
            public Data Data { get; set; }
        }
        public class Data {
            public Dictionary<string, Inverter> Inverters { get; set; }
            public Site Site { get; set; }
            string Version { get; set; }
        }
        public class Inverter {
            [JsonPropertyName("DT")]
            public int DeviceType { get; set; }

            [JsonPropertyName("E_Day")]
            public double? EnergyDay { get; set; }

            [JsonPropertyName("E_Year")]
            public double? EnergyYear { get; set; }

            [JsonPropertyName("E_Total")]
            public double? EnergyTotal { get; set; }

            [JsonPropertyName("P")]
            public double? Power { get; set; }
        }
        public class Site {
            [JsonPropertyName("E_Day")]
            public double? EnergyDay { get; set; }

            [JsonPropertyName("E_Year")]
            public double? EnergyYear { get; set; }

            [JsonPropertyName("E_Total")]
            public double? EnergyTotal { get; set; }

            [JsonPropertyName("Meter_Location")]
            public string MeterLocation { get; set; }

            [JsonPropertyName("Mode")]
            public string Mode { get; set; }

            [JsonPropertyName("P_Akku")]
            public double? AccumulatorPower { get; set; }

            [JsonPropertyName("P_Grid")]
            public double? GridPower { get; set; }

            [JsonPropertyName("P_Load")]
            public double? LoadPower { get; set; }

            [JsonPropertyName("P_PV")]
            public double? ArrayPower { get; set; }

            [JsonPropertyName("rel_Autonomy")]
            public double? AutonomyPercent { get; set; }

            [JsonPropertyName("rel_SelfConsumption")]
            public double? SelfConsumptionPercent { get; set; }
        }
    }
}
