using System.Net.Http;
using Moq;
using Xunit;
using Serilog.Sinks.Logzio.Tests.Support;
using Serilog.Sinks.Logzio;

namespace Serilog.Sinks.Http.Tests.Sinks.Http
{
    public class HttpLogzioTest
    {
        private readonly Mock<IHttpClient> client;
        private readonly string autkey;
        private readonly LogzioSink sink;

        public HttpLogzioTest()
        {
            client = new Mock<IHttpClient>();
            autkey = "Key";
            sink = new LogzioSink(
                client.Object,
                autkey,
                LogzioSink.DefaultBatchPostingLimit,
                LogzioSink.DefaultPeriod,
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
                    "http://listener.logz.io:8070/?token=" + autkey,
                    It.IsAny<HttpContent>()),
                Times.Once);
        }
    }
}
