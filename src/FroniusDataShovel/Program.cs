using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Client = FroniusSolarApiClient.FroniusSolarApiClient;
using FroniusSolarApiClient.Endpoints.GetPowerFlowRealtimeData;

namespace FroniusDataShovel
{
    class Program
    {
        private static readonly HttpClient __httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            await foreach (var client in DiscoverInverterClients()) {
                Console.WriteLine($"Inverter discovered at: {client.Address}");
            }
        }

        static async IAsyncEnumerable<Client> DiscoverInverterClients() {
            foreach (var task in GetResponsesFromGatewayClients()) {
                var (client, response) = await task;
                if (response != null) {
                    yield return client;
                }
            }
        }

        static IEnumerable<Task<(Client, Response)>> GetResponsesFromGatewayClients() {
            return StartGatewayTasks().ToArray();
            static IEnumerable<Task<(Client, Response)>> StartGatewayTasks() {
                foreach (var ip in GetGatewayAddresses()) {
                    if (ip.AddressFamily != AddressFamily.InterNetwork) {
                        continue;
                    }
                    var segments = ip.ToString().Split(".");
                    foreach (byte sub in Enumerable.Range(0, 256)) {
                        yield return Task.Run(async () => await GetClientAndResponse($"{segments[0]}.{segments[1]}.{segments[2]}.{sub}"));
                    }
                }
            }
        }

        static async Task<(Client, Response)> GetClientAndResponse(string address) {
            var client = new Client(__httpClient) { Address = address };
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
