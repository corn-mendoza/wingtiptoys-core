using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using WingtipToys.Models;

namespace WingtipToys.Client
{
    public class GetAllProductsCommand : HystrixCommand<List<Product>>
    {
        public IProductService _productService;
        public ILogger<GetAllProductsCommand> _logger;

        private string _name;
        public GetAllProductsCommand(IProductService productService, string name, ILogger<GetAllProductsCommand> logger)
            : base(HystrixCommandGroupKeyDefault.AsKey("ProductCircuitBreakerGroup"))
        {
            _name = name;
            _productService = productService;
            _logger = logger;

            IsFallbackUserDefined = true;
        }
        protected override async Task<List<Product>> RunAsync()
        {
            return await _productService.GetAllProductsAsync();
        }
        protected override Task<List<Product>> RunFallbackAsync()
        {
            _logger.LogInformation("Running Get All Products Fallback");

            return Task.FromResult(new List<Product>());
        }
    }
}
