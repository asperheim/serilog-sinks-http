// Copyright 2015-2016 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using static System.String;
using static Newtonsoft.Json.Formatting;
using static Newtonsoft.Json.JsonConvert;

namespace Serilog.Sinks.Logzio
{
    /// <summary>
    /// Send log events using HTTP POST over the network.
    /// </summary>
    public sealed class LogzioSink : PeriodicBatchingSink
    {
        private IHttpClient client;

        private readonly string requestUri;

        /// <summary>
        /// The default batch posting limit.
        /// </summary>
        public static int DefaultBatchPostingLimit { get; } = 1000;

        /// <summary>
        /// The default period.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        /// <summary>
        /// 
        /// </summary>
        private const string LogzIOUrl = "http://listener.logz.io:8070/?token={0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="LogzioSink"/> class. 
        /// </summary>
        /// <param name="client">The client responsible for sending HTTP POST requests.</param>
        /// <param name="authToken">The token for logzio.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        public LogzioSink(
            IHttpClient client,
            string authToken,
            int batchPostingLimit,
            TimeSpan period,
            IFormatProvider formatProvider)
            : base(batchPostingLimit, period)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (authToken == null)
                throw new ArgumentNullException(nameof(authToken));

            this.client = client;

            requestUri = Format(LogzIOUrl, authToken);
        }

        #region PeriodicBatchingSink Members

        /// <summary>
        /// Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events">The events to emit.</param>
        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var payload = FormatPayload(events);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var result = await client
                .PostAsync(requestUri, content)
                .ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
                throw new LoggingFailedException($"Received failed result {result.StatusCode} when posting events to {requestUri}");
        }

        /// <summary>
        /// Free resources held by the sink.
        /// </summary>
        /// <param name="disposing">
        /// If true, called because the object is being disposed; if false, the object is being
        /// disposed from the finalizer.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing || client == null)  return;

            client.Dispose();
            client = null;
        }

        #endregion

        private string FormatPayload(IEnumerable<LogEvent> events)
        {
            var result = events
                .Select(FormatLogEvent)
                .ToArray();

            return Join(",\n", result);
        }

        private string FormatLogEvent(LogEvent curEvent)
        {
            return SerializeObject(curEvent, None);
        }
    }
}