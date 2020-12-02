using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FroniusSolarApi {
	public interface IClient {
		protected HttpClient HttpClient { get; }
		public Uri BaseUri { get; }
		public sealed async Task<TObject?> GetDeserializedObject<TObject>(string relativeUri, CancellationToken cancellationToken) {
			var stream = await HttpClient.GetStreamAsync(new Uri(BaseUri, relativeUri), cancellationToken);
			return await JsonSerializer.DeserializeAsync<TObject>(stream);
		}
	}
}
