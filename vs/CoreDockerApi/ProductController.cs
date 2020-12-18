using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreDockerApi
{
    public class ProductController : Controller
    {
        [HttpGet("/products")]
        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            return await Task.FromResult(new List<ProductModel> 
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
                }
            });
        }
    }
}
