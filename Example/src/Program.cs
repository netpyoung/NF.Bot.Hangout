using System;
using System.Threading.Tasks;
using NF.Bot.Hangout;

namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Discovery API Sample");
            Console.WriteLine("====================");
            RunnerArgs runnerArgs = new RunnerArgs(
                pubSubProjectId: "pub-sub-project-id",
                pubSubSubscriptionId: "pub-sub-subscription-id",
                serviceAccountJsonFpath: "credential-json-fpath"
            );
            Handler handler = new Handler();
            Runner runner = new Runner(args: runnerArgs, handler: handler);
            
            Console.WriteLine("Runnning");
            var _ = runner.Start();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            runner.Stop().Wait();
            Console.WriteLine("Stopped");
        }
    }
}
