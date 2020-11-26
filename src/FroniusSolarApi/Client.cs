using System;
using System.Net.Http;

namespace FroniusSolarApi {
	public class Client : IClient {
		protected HttpClient HttpClient { get; }
		private Uri BaseUri { get; }
		HttpClient IClient.HttpClient => HttpClient;
		Uri IClient.BaseUri => BaseUri;

		public Client(HttpClient httpClient, Uri baseUri) {
			HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
		}
	}
}