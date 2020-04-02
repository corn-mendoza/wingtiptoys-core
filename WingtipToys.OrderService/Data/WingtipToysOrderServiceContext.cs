using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WingtipToys.Models;

namespace WingtipToys.OrderService.Data
{
    public class WingtipToysOrderServiceContext : DbContext
    {
        public WingtipToysOrderServiceContext (DbContextOptions<WingtipToysOrderServiceContext> options)
            : base(options)
        {
        }

        public DbSet<WingtipToys.Models.Order> Order { get; set; }

        public DbSet<WingtipToys.Models.OrderDetail> OrderDetail { get; set; }
    }
}
