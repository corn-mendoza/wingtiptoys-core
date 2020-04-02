using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using WingtipToys.Models;

namespace WingtipToys.Client
{
    public class GetProductsCommand : HystrixCommand<List<Product>>
    {
        public IProductService _productService;
        public ILogger<GetProductsCommand> _logger;

        public string Catagory { get { return _cat; } set{ _cat = value; } }

        private string _cat = string.Empty;
        private IDistributedCache _cache;
        public GetProductsCommand(
            IHystrixCommandOptions options, 
            IProductService productService, 
            ILogger<GetProductsCommand> logger,
            IDistributedCache cache) 
                : base(options)
        {
            _productService = productService;
            _logger = logger;

            IsFallbackUserDefined = true;
        }
        protected override async Task<List<Product>> RunAsync()
        {
            return await _productService.GetProductsAsync(_cat);
        }
        protected override async Task<List<Product>> RunFallbackAsync()
        {
            _logger.LogInformation("Running Get Products Fallback");

            return await Task.FromResult(new List<Product>());
        }
    }
}
