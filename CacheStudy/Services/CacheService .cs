using CacheStudy.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CacheStudy.Services
{
    public class CacheService
    {
        private ICustomCache m_Memory;
        private ICustomCache m_Redis;

        public CacheService(ICustomCache memoryCache, ICustomCache distributedCache)
        {
            m_Memory = memoryCache;
            m_Redis = distributedCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ProductDto GetProductByIdFromRedis(int id)
        {
            string cacheKey = "productId=" + id;

            ProductDto product = m_Redis.readDataFromCache(cacheKey);
            if (product != null)
                return product;

            DataAccess dao = new DataAccess();
            product = dao.GetProductById(id);

            m_Redis.writeDataToCache(cacheKey, product);

            return product;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ProductDto GetProductByIdFromMemory(int id)
        {
            var cacheKey = "productId=" + id;
            ProductDto returnObj = m_Memory.readDataFromCache(cacheKey);
            if (returnObj != null)
                return returnObj;

            returnObj = GetProductByIdFromRedis(id);
            m_Memory.writeDataToCache(cacheKey, returnObj);
            return returnObj;
        }

        public ProductDto GetProductById(int id)
        {
            return GetProductByIdFromMemory(id);
        }
    }
}
