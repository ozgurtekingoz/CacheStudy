using CacheStudy.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CacheStudy.Services
{
    public class RedisService : ICustomCache
    {
        private readonly IDistributedCache m_DistributedCache;
        public RedisService(IDistributedCache distributedCache)
        {
            m_DistributedCache = distributedCache;
        }
        public ProductDto readDataFromCache(string cacheKey)
        {
            string serializedProduct;
            var product = m_DistributedCache.Get(cacheKey);
            if (product != null)
            {
                serializedProduct = Encoding.UTF8.GetString(product);
                return JsonConvert.DeserializeObject<ProductDto>(serializedProduct);
            }
            return null;
        }
        public void writeDataToCache(string cacheKey, ProductDto data)
        {
            var product = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(30)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
            m_DistributedCache.Set(cacheKey, product, options);
        }
    }
}
