using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WingtipToys.Client;
using WingtipToys.Models;

namespace WingtipToysUI.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _cache;
        //private GetAllProductsCommand _getAllProductsCommand;
        private GetProductsCommand _getProductsCommand;
        private GetProductCommand _getProductCommand;
        private ILogger<ProductsController> _logger;
        private IOptionsSnapshot<WingtipToysProductServiceOptions> _options;

        public ProductsController(ILogger<ProductsController> logger, IOptionsSnapshot<WingtipToysProductServiceOptions> options, GetProductCommand cmd1, GetProductsCommand cmd2,
            [FromServices] IDistributedCache cache)
        {
            _logger = logger;
            _options = options;
            _cache = cache;
            _getProductCommand = cmd1;
            _getProductsCommand = cmd2;
        }

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Catalog(string cat)
        {
            
            string catName = cat;

            ViewData["Category"] = catName;
            _getProductsCommand.Catagory = catName;

            return View(await _getProductsCommand.ExecuteAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            _getProductCommand.ProductID = id;
            return View(await _getProductCommand.ExecuteAsync());
        }
    }
}