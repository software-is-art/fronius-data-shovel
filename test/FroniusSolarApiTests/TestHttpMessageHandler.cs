using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FroniusSolarApiTests
{
    class TestHttpMessageHandler : HttpMessageHandler
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
        public string Content { get; init; } = "{'hello':'there'}";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.Run(() => new HttpResponseMessage(StatusCode) { Content = new TestHttpContent { Content = Content } });

        private class TestHttpContent : HttpContent
        {
            public string Content { get; init; }
            private byte[] Bytes => Encoding.UTF8.GetBytes(Content);

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) => stream.WriteAsync(new ReadOnlyMemory<byte>(Bytes)).AsTask();
            protected override bool TryComputeLength(out long length)
            {
                length = Bytes.Length;
                return true;
            }
        }
    }
}
