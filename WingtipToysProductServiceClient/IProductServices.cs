using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WingtipToys.Models;

namespace WingtipToys.Client
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product> GetProductAsync(int id);
        public Task<List<Product>> GetProductsAsync(string name);
    }
}
