using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.SQSEvents;

using LambdaHandlers;

namespace LambdaHandlers.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestSQSEventLambdaFunction()
        {
            var sqsEvent = new SQSEvent
            {
                Records = new List<SQSEvent.SQSMessage>
                {
                    new SQSEvent.SQSMessage
                    {
                        Body = "foobar"
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

            Assert.Contains("Processed message foobar", logger.Buffer.ToString());
        }
    }
}
