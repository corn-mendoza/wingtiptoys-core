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
    public class GetProductCommand : HystrixCommand<Product>
    {
        public IProductService _productService;
        public ILogger<GetProductCommand> _logger;

        public int ProductID { get { return _productId; } set{ _productId = value; } }

        private int _productId;
        private IDistributedCache _cache;
        public GetProductCommand(IHystrixCommandOptions options, IProductService productService, ILogger<GetProductCommand> logger, IDistributedCache cache) 
            : base(options)
        {
            _productService = productService;
            _logger = logger;
            _cache = cache;

            IsFallbackUserDefined = true;
        }
        protected override async Task<Product> RunAsync()
        {
            return await _productService.GetProductAsync(_productId);
        }
        protected override async Task<Product> RunFallbackAsync()
        {
            _logger.LogInformation($"Running Get Product Fallback - Product ID {_productId} not found.");

            return await Task.FromResult(new Product());
        }
    }
}
