using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.SQSEvents;

using LambdaHandlers;
using System.Text.Json;
using Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData;

namespace LambdaHandlers.Tests
{
    public class FunctionTest
    {

        private static string ResponseObject { get; } =
@"{
   ""Body"" : {
      ""Data"" : {
         ""Inverters"" : {
            ""1"" : {
               ""DT"" : 76,
               ""E_Day"" : 30152,
               ""E_Total"" : 158419.015625,
               ""E_Year"" : 158419,
               ""P"" : 656
            }
         },
         ""Site"" : {
            ""E_Day"" : 30152,
            ""E_Total"" : 158419.015625,
            ""E_Year"" : 158419,
            ""Meter_Location"" : ""grid"",
            ""Mode"" : ""meter"",
            ""P_Akku"" : null,
            ""P_Grid"" : -143.25999999999999,
            ""P_Load"" : -512.74000000000001,
            ""P_PV"" : 656,
            ""rel_Autonomy"" : 100,
            ""rel_SelfConsumption"" : 78.161585365853654
         },
         ""Version"" : ""12""
      }
   },
   ""Head"" : {
      ""RequestArguments"" : { 
            ""foo"":""bar""
        },
      ""Status"" : {
         ""Code"" : 0,
         ""Reason"" : ""Aliens"",
         ""UserMessage"" : ""Sorry""
      },
      ""Timestamp"" : ""2020-11-26T18:28:39+13:00""
   }
}";

        [Fact]
        public async Task TestSQSEventLambdaFunction()
        {
            var sqsEvent = new SQSEvent
            {
                Records = new List<SQSEvent.SQSMessage>
                {
                    new SQSEvent.SQSMessage
                    {
                        Body = ResponseObject
                    }
                }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };

            var function = new FroniusRealtimeHandler();
            await function.FunctionHandler(sqsEvent, context);

            Assert.Equal($"Roundtripped serialization result: {JsonSerializer.Serialize(JsonSerializer.Deserialize<Response>(ResponseObject))}\r\n", logger.Buffer.ToString());
        }
    }
}
