
using System;

namespace Serilog.Sinks.Logzio.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.LiterateConsole()
                .WriteTo.Logzio("<you key>", 1)
                .CreateLogger();

            while (true)
            {
                System.Threading.Thread.Sleep(5000);
                Log.Warning("Warning fra logz.io sink");
            }

        }
    }
}
