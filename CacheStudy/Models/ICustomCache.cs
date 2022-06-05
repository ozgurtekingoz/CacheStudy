using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheStudy.Models
{
    public interface ICustomCache
    {
        ProductDto readDataFromCache(string cacheKey);
        void writeDataToCache(string cacheKey, ProductDto data);
    }
}
