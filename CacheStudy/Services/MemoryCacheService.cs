using CacheStudy.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheStudy.Services
{
    public class MemoryCacheService : ICustomCache
    {
        private readonly IMemoryCache m_MemoryCache;
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            m_MemoryCache = memoryCache;
        }

        public ProductDto readDataFromCache(string cacheKey)
        {
            if (m_MemoryCache.TryGetValue(cacheKey, out ProductDto productDto))
                return productDto;

            return null;
        }
        public void writeDataToCache(string cacheKey, ProductDto data)
        {
            m_MemoryCache.Set(cacheKey, data, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                Priority = CacheItemPriority.Normal
            });
        }
    }
}
