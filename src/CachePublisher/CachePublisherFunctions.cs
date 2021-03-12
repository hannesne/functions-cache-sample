using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace CachePublisher
{
    public class CachePublisherFunctions
    {
        private const string HubName = "cacheHub";
        private const string CacheUpdateMessage = "cache_update";

        [FunctionName(nameof(UpdateCache))]
        public async Task UpdateCache(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [SignalR(HubName = HubName)] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            string cacheItem = await req.ReadAsStringAsync();
            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = CacheUpdateMessage,                    
                    Arguments = new[] { cacheItem }
                });
        }

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = HubName)] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}