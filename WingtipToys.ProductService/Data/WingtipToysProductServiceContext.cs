using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WingtipToys.Models;

namespace WingtipToys.ProductService.Data
{
    public class WingtipToysProductServiceContext : DbContext
    {
        public WingtipToysProductServiceContext (DbContextOptions<WingtipToysProductServiceContext> options)
            : base(options)
        {
        }

        public DbSet<WingtipToys.Models.Product> Product { get; set; }

        public DbSet<WingtipToys.Models.Category> Category { get; set; }
    }
}
