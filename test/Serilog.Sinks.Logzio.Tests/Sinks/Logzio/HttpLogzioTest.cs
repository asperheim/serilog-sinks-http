using System.Net.Http;
using System.Threading;
using Moq;
using Serilog.Sinks.Logzio.Tests.Support;
using Serilog.Sinks.Logzio;
using Xunit;

namespace Serilog.Sinks.Http.Tests.Sinks.Logzio
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

            // TODO: Hear if aynone has got a better idea to get this pos to run.
            Thread.Sleep(50);

            // Assert
            client.Verify(
                mock => mock.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<HttpContent>()),
                Times.Once);
        }
    }
}
