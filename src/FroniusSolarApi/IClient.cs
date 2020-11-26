using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FroniusSolarApi {
	public interface IClient {
		protected HttpClient HttpClient { get; }
		public Uri BaseUri { get; }
		public sealed async Task<TObject?> GetDeserializedObject<TObject>(string relativeUri) {
			var stream = await HttpClient.GetStreamAsync(new Uri(BaseUri, relativeUri));
			return await JsonSerializer.DeserializeAsync<TObject>(stream);
		}
	}
}
