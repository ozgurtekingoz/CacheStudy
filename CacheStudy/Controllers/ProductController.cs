using CacheStudy.Models;
using CacheStudy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheStudy.Controllers
{
    [Route("api/productcontroller")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        [HttpGet("getById/{id}")]
        public JsonResult Get(int id)
        {
            DataAccess dao = new DataAccess();
            return new JsonResult(dao.GetProductById(id));
        }

        [HttpGet("getAll")]
        public JsonResult Get()
        {
            DataAccess dao = new DataAccess();
            return new JsonResult(dao.GetAllProducts());
        }

        [HttpGet("getAllFromCacheById/{id}")]
        public ProductDto getAllFromCacheById(int id)
        {
            CacheService css = new CacheService(new MemoryCacheService(_memoryCache), new RedisService(_distributedCache));
            return css.GetProductById(id);
        }
    }
}
