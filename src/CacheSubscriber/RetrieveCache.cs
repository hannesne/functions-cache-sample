using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CacheSubscriber
{
    public class RetrieveCacheFunctions
    {
        private readonly ICache cache;

        public RetrieveCacheFunctions(ICache cache)
        {
            this.cache = cache;
        }

        [Function(nameof(RetrieveCache))]
        public HttpResponseData RetrieveCache(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {

            HttpResponseData httpResponseData = req.CreateResponse(System.Net.HttpStatusCode.OK);
            httpResponseData.WriteString(String.Join(",", cache.Items));
            return httpResponseData;
        }
    }
}