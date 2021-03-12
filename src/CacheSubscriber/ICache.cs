using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheSubscriber
{    
    public interface ICache
    {
        IReadOnlyCollection<string> Items { get; }

        Task Add(string cacheItem);
    }
}