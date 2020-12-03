using System.Text.Json;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaHandlers {
	public class FroniusRealtimeHandler
    {
        public static string HandlerName { get; } = $"{nameof(LambdaHandlers)}::{nameof(LambdaHandlers)}.{nameof(FroniusRealtimeHandler)}::{nameof(FunctionHandler)}";

        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            var response = JsonSerializer.Deserialize<Response>(message.Body);
            context.Logger.LogLine($"Roundtripped serialization result: {JsonSerializer.Serialize(response)}");
            await Task.CompletedTask;
        }
    }
}
