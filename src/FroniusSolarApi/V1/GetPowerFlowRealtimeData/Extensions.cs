using System.Threading.Tasks;

namespace FroniusSolarApi.V1.GetPowerFlowRealtimeData {
	public static class Extensions {
        public static async Task<Response?> GetPowerFlowRealtimeData(this IClient client) {
            const string relativeUri = "/solar_api/v1/GetPowerFlowRealtimeData.fcgi";
            return await client.GetDeserializedObject<Response>(relativeUri);
        }
    }
}
