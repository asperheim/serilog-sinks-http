using System.Net.Http;
using Moq;
using Xunit;

namespace Serilog.Sinks.Http.Tests.Sinks.Http
{
    public class HttpLogzioTest
    {
        private readonly Mock<IHttpClient> client;
        private readonly string requestUri;
        private readonly LogzioSink sink;

        public HttpLogzioTest()
        {
            client = new Mock<IHttpClient>();
            requestUri = "www.mylogs.com";
            sink = new HttpSink(
                client.Object,
                requestUri,
                HttpSink.DefaultBatchPostingLimit,
                HttpSink.DefaultPeriod,
                null);
        }

        [Fact]
        public void RequestUri()
        {
            // Act
            sink.Emit(Some.DebugEvent());

            // Assert
            client.Verify(
                mock => mock.PostAsync(
                    requestUri,
                    It.IsAny<HttpContent>()),
                Times.Once);
        }
    }
}
