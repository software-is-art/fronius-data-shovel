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
using FroniusDataShovel.SolarApi.GetPowerFlowRealtimeData;

namespace FroniusDataShovel
{
    class Program
    {
        private static readonly HttpClient __httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            var tasks = GetResponsesFromGatewayClients().ToArray();
            foreach (var task in tasks) {
                var (uri, response) = await task;
                Console.WriteLine($"{uri} | {response}");
            }
        }

        static IEnumerable<Task<(string, Response)>> GetResponsesFromGatewayClients() {
            foreach (var ip in GetGatewayAddresses()) {
                if (ip.AddressFamily != AddressFamily.InterNetwork) {
                    continue;
                }
                var segments = ip.ToString().Split(".");
                foreach (byte sub in Enumerable.Range(0, 256)) {
                    yield return Task.Run(async () => await GetResponse($"http://{segments[0]}.{segments[1]}.{segments[2]}.{sub}/solar_api/v1/GetPowerFlowRealtimeData.fcgi"));
                }
            }
        }
        static async Task<(string, Response)> GetResponse(string uri) {
            try {
                var stream = await __httpClient.GetStreamAsync(uri);
                return (uri, await JsonSerializer.DeserializeAsync<Response>(stream));
            } catch {
                return (uri, null);
            }
        }

        static IEnumerable<IPAddress> GetGatewayAddresses() {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            return new HashSet<IPAddress>(adapters.SelectMany(a => a.GetIPProperties().GatewayAddresses.Select(a => a.Address)));
        }
    }
}
