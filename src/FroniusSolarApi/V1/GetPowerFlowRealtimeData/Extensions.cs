using Models.FroniusSolarApi.V1.GetPowerFlowRealtimeData;
using System.Threading;
using System.Threading.Tasks;

namespace FroniusSolarApi.V1.GetPowerFlowRealtimeData {
	public static class Extensions {
        public static async Task<Response?> GetPowerFlowRealtimeData(this IClient client, CancellationToken cancellationToken) {
            const string relativeUri = "/solar_api/v1/GetPowerFlowRealtimeData.fcgi";
            return await client.GetDeserializedObject<Response>(relativeUri, cancellationToken);
        }
    }
}
