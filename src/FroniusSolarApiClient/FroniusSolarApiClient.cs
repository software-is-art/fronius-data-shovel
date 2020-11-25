using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FroniusSolarApiClient.Endpoints.GetPowerFlowRealtimeData;

namespace FroniusSolarApiClient {
    public class FroniusSolarApiClient {
        private HttpClient HttpClient { get; }
        public FroniusSolarApiClient(HttpClient httpClient) => HttpClient = httpClient;
        public string Address { get; init; }
        public async Task<Response> GetPowerFlowRealtimeData() {
            var uri = $"http://{Address}/solar_api/v1/GetPowerFlowRealtimeData.fcgi";
            var stream = await HttpClient.GetStreamAsync(uri);
            return await JsonSerializer.DeserializeAsync<Response>(stream);
        }
    }
}