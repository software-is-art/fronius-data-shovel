using FroniusSolarApi;
using FroniusSolarApi.V1.GetPowerFlowRealtimeData;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FroniusSolarApiTests.V1
{
    public class GetPowerFlowRealtimeDataTests
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
        public async Task DeserializerIsSuccessful()
        {
            IClient client = new Client(
                new HttpClient(
                    new TestHttpMessageHandler
                    {
                        Content = ResponseObject
                    }
               ), new Uri("http://127.0.0.1")
            );

            (await client.GetPowerFlowRealtimeData(default)).Validate(
                response =>
                {
                    response.Body.Validate(body =>
                    {
                        body.Data.Validate(data =>
                        {
                            data.Inverters.Validate(inverters =>
                            {
                                Assert.True(inverters.TryGetValue("1", out var inverter));
                                Assert.Equal(76, inverter.DeviceType);
                                Assert.Equal(30152, inverter.EnergyDay);
                                Assert.Equal(158419.015625, inverter.EnergyTotal);
                                Assert.Equal(158419, inverter.EnergyYear);
                                Assert.Equal(656, inverter.Power);
                            });
                            data.Site.Validate(site =>
                            {
                                Assert.Equal(30152, site.EnergyDay);
                                Assert.Equal(158419.015625, site.EnergyTotal);
                                Assert.Equal(158419, site.EnergyYear);
                                Assert.Equal("grid", site.MeterLocation);
                                Assert.Equal("meter", site.Mode);
                                Assert.Null(site.AccumulatorPower);
                                Assert.Equal(-143.25999999999999, site.GridPower);
                                Assert.Equal(-512.74000000000001, site.LoadPower);
                                Assert.Equal(656, site.ArrayPower);
                                Assert.Equal(100, site.AutonomyPercent);
                                Assert.Equal(78.161585365853654, site.SelfConsumptionPercent);
                            });
                            Assert.Equal("12", data.Version);
                        });
                    });
                    response.Head.Validate(head =>
                    {
                        head.RequestArguments.Validate(requestArgs =>
                        {
                            Assert.True(requestArgs.TryGetValue("foo", out var value));
                            Assert.Equal("bar", value);
                        });
                        head.Status.Validate(status =>
                        {
                            Assert.Equal(0, status.Code);
                            Assert.Equal("Aliens", status.Reason);
                            Assert.Equal("Sorry", status.UserMessage);
                        });
                    });
                }
            );
        }
    }
}
