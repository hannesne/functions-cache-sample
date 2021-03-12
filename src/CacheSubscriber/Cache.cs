using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace CacheSubscriber
{
    public class Cache : ICache
    {
        private const string HubName = "cacheHub";
        private const string CacheUpdateMessage = "cache_update";
        private readonly List<string> cacheItems = new List<string>(){"seed1", "seed2"};
        
        public IReadOnlyCollection<string> Items => cacheItems;

        private static Cache instance;
        private HubConnection connection;

        public static async Task<Cache> Instance() 
        {
            if (instance == null) 
            {
                instance = new Cache();

                try {
                    Console.WriteLine($"Connecting to publisher");
                    await instance.connection.StartAsync();
                    Console.WriteLine("SignalR Connection Started.");
                } catch (Exception ex) {
                    Console.WriteLine("Signal R Connection Failed!");
                    Console.WriteLine(ex);
                    throw;
                }
                

            }
            return instance;
        }
        private Cache()
        {

            string publisherUrl = Environment.GetEnvironmentVariable("PublisherUri");
            Console.WriteLine($"Publisher Url: {publisherUrl}");
            connection = new HubConnectionBuilder()
                .WithUrl(publisherUrl)
                .WithAutomaticReconnect()
                .Build();
            
            connection.On<object>(CacheUpdateMessage, cacheItem =>
            {
                Console.WriteLine($"message received: {cacheItem}");
                cacheItems.Add(cacheItem.ToString());
            });
        }

        public Task Add(string cacheItem)
        {
            throw new NotImplementedException();
        }
    }
}