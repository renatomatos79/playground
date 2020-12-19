using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDockerApi.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreDockerApi
{
    public class ProductController : Controller
    {
        private readonly ICacheManager cacheManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProductController(ICacheManager cacheManager, IHttpContextAccessor httpContextAccessor)
        {
            this.cacheManager = cacheManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("/products")]
        public async Task<IActionResult> GetProductsAsync()
        {
            var cache = cacheManager.Get<List<ProductModel>>("products");
            if (cache != null && cache.Item != null)
            {
                var headers = this.httpContextAccessor.HttpContext.Response.GetTypedHeaders();
                headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(cache.MaxAgeInSeconds)
                };
                return await Task.FromResult(Ok(cache.Item));
            }
            var result = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = 1,
                    Name = "Eggs (box with 12 units)",
                    Price = (decimal)1.50
                },
                new ProductModel
                {
                    Id = 2,
                    Name = "Chocolate",
                    Price = (decimal)1.99
                },
                new ProductModel
                {
                    Id = 3,
                    Name = "Butter (President)",
                    Price = (decimal)2.25
                },
                new ProductModel
                {
                    Id = 4,
                    Name = "Codfish (Pascoal)",
                    Price = (decimal)12.25
                },
                new ProductModel
                {
                    Id = 5,
                    Name = "Cheese 500g (Flamingo)",
                    Price = (decimal)2.40
                },
                new ProductModel
                {
                    Id = 6,
                    Name = "Yogurt Danone (6 units)",
                    Price = (decimal)1.45
                },
                new ProductModel
                {
                    Id = 7,
                    Name = "Bread (6 units)",
                    Price = (decimal)0.45
                }
            };
            return await Task.FromResult(Ok(result));
        }

        [HttpPut("/products/{cache}")]
        public async Task<IActionResult> PutProductsAsync([FromRoute] string cache, [FromBody] List<ProductModel> products)
        {
            this.cacheManager.Add(cache, products, 30);
            return await Task.FromResult(NoContent());
        }
    }
}
