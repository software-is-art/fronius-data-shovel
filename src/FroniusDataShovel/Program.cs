using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using CommandLine;
using FroniusSolarApi;
using FroniusSolarApi.V1.GetPowerFlowRealtimeData;
using Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData;

namespace FroniusDataShovel
{
    class Program
    {
        private static readonly HttpClient __httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            var accountId = Environment.GetEnvironmentVariable("AWS_ACCOUNT_ID");
            if (string.IsNullOrEmpty(accountId))
            {
                throw new Exception("AWS_ACCOUNT_ID must be non null and valid");
            }
            try
            {
                var source = new CancellationTokenSource();
                var client = await DiscoverInverter(source.Token);
                source.Cancel();
                await PushToSQS(client, accountId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine($"{e.StackTrace}");
            }
        }

        static async Task PushToSQS(IClient client, string accountId)
        {
            var source = new CancellationTokenSource();
            var sqsClient = new AmazonSQSClient(RegionEndpoint.USEast1);
            var urlRequest = new GetQueueUrlRequest
            {
                QueueName = AWSConstructs.Names.FroniusIngressQueue,
                QueueOwnerAWSAccountId = accountId
            };
            var url = (await sqsClient.GetQueueUrlAsync(urlRequest))?.QueueUrl;
            if (url == null)
            {
                throw new Exception($"Failed to get SQS URL for: {JsonSerializer.Serialize(urlRequest)}");
            }
            Response? dataLast = null;
            while (true)
            {
                var data = await client.GetPowerFlowRealtimeData(source.Token);
                if (data?.Head.Timestamp == dataLast?.Head.Timestamp)
                {
                    await Task.Delay(10);
                    continue;
                }
                var dataJson = JsonSerializer.Serialize(data);
                var message = new SendMessageRequest(url, dataJson);
                var response = await sqsClient.SendMessageAsync(message);
                Console.WriteLine($"{response.HttpStatusCode} | {response.MessageId} | {dataJson}");
                dataLast = data;
            }
        }

        static async Task<IClient> DiscoverInverter(CancellationToken cancellationToken)
        {
            var scan = new LinkedList<Task<(IClient, Response?)>>(GetResponsesFromGatewayClients(cancellationToken));
            while (scan.Count > 0)
            {
                await Task.Delay(1000);
                var node = scan.First;
                while (node != null)
                {
                    var task = node.Value;
                    if (task != null && task.IsCompleted)
                    {
                        var (client, response) = await task;
                        if (response != null)
                        {
                            return client;
                        }
                        scan.Remove(node);
                    }
                    node = node.Next;
                }
            }
            throw new Exception("No inverters discovered on local network");
        }

        static IEnumerable<Task<(IClient, Response?)>> GetResponsesFromGatewayClients(CancellationToken cancellationToken)
        {
            foreach (var ip in GetGatewayAddresses())
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }
                Console.WriteLine($"Gateway found: {ip}");
                var segments = ip.ToString().Split(".");
                foreach (byte sub in Enumerable.Range(0, 256))
                {
                    yield return Task.Run(async () => await GetClientAndResponse(new Uri($"http://{segments[0]}.{segments[1]}.{segments[2]}.{sub}"), cancellationToken));
                }
            }
        }

        static async Task<(IClient, Response?)> GetClientAndResponse(Uri baseUri, CancellationToken cancellationToken)
        {
            var client = new Client(__httpClient, baseUri);
            try
            {
                var response = await client.GetPowerFlowRealtimeData(cancellationToken);
                return (client, response);
            }
            catch
            {
                return (client, null);
            }
        }

        static IEnumerable<IPAddress> GetGatewayAddresses()
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            return new HashSet<IPAddress>(adapters.SelectMany(a => a.GetIPProperties().GatewayAddresses.Select(a => a.Address)));
        }
    }
}
