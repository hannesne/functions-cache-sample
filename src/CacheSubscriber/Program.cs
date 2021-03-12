using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheSubscriber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(c =>
                {
                    c.AddCommandLine(args);
                    c.AddEnvironmentVariables();                    
                })
                .ConfigureFunctionsWorker((c, b) =>
                {
                    b.UseFunctionExecutionMiddleware();
                    
                })
                .ConfigureServices(services => {
                    services.AddSingleton<ICache>(Cache.Instance().Result);
                })
                .Build();

            await host.RunAsync();
        }
    }
}