using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using FroniusSolarApi;
using FroniusSolarApi.V1.GetPowerFlowRealtimeData;

namespace FroniusDataShovel
{
    class Program
    {
        private static readonly HttpClient __httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            await foreach (var client in DiscoverInverterClients()) {
                Console.WriteLine($"Inverter discovered at: {client.BaseUri}");
            }
        }

        static async IAsyncEnumerable<IClient> DiscoverInverterClients() {
            foreach (var task in GetResponsesFromGatewayClients()) {
                var (client, response) = await task;
                if (response != null) {
                    yield return client;
                }
            }
        }

        static IEnumerable<Task<(IClient, Response?)>> GetResponsesFromGatewayClients() {
            return StartGatewayTasks().ToArray();
            static IEnumerable<Task<(IClient, Response?)>> StartGatewayTasks() {
                foreach (var ip in GetGatewayAddresses()) {
                    if (ip.AddressFamily != AddressFamily.InterNetwork) {
                        continue;
                    }
                    var segments = ip.ToString().Split(".");
                    foreach (byte sub in Enumerable.Range(0, 256)) {
                        yield return Task.Run(async () => await GetClientAndResponse(new Uri($"http://{segments[0]}.{segments[1]}.{segments[2]}.{sub}")));
                    }
                }
            }
        }

        static async Task<(IClient, Response?)> GetClientAndResponse(Uri baseUri) {
            var client = new Client(__httpClient, baseUri);
            try {
                return (client, await client.GetPowerFlowRealtimeData());
            } catch {
                return (client, null);
            }
        }

        static IEnumerable<IPAddress> GetGatewayAddresses() {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            return new HashSet<IPAddress>(adapters.SelectMany(a => a.GetIPProperties().GatewayAddresses.Select(a => a.Address)));
        }
    }
}
