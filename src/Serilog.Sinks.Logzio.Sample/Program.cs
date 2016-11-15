
using System.Diagnostics;

namespace Serilog.Sinks.Logzio.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.LiterateConsole()
                .WriteTo.Logzio("http://listener.logz.io:8070/?token=zXqxXdfelIrNmynmdiKncZJXgvVERyUv",1)
                .CreateLogger();

            Log.Verbose("Dette er veldig buggy altså!");
            Debug.WriteLine("Nisselue");
            Log.Warning("Warning fra logz.io sink");

            
        }
    }
}
