using Microsoft.EntityFrameworkCore;
using System;

namespace WingtipToys.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext()
          //: base(ConnectionString)
        {
            //this.SetCommandTimeOut(30);
        }

        public static string ConnectionString
        {
            get
            {
                //try
                //{
                //    CFEnvironmentVariables _env = new CFEnvironmentVariables(ServerConfig.Configuration);
                //    var _connect = _env.getConnectionStringForDbService("user-provided", "wingtiptoysdb");
                //    if (!string.IsNullOrEmpty(_connect))
                //    {
                //        Console.WriteLine($"Using connection string '{_connect}' for catalog");
                //        return _connect;
                //    }
                //}
                //catch { }

                //var _s = System.Configuration.ConfigurationManager.ConnectionStrings["WingtipToys"].ConnectionString;
                //Console.WriteLine($"Using default connection string for catalog {_s}");

                return string.Empty;
            }
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}