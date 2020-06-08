// <autogenerated />
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Logging;
using Steeltoe.Extensions.Configuration.ConfigServer;
namespace WingtipToys.OrderManager
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var bld = CreateWebHostBuilder(args).Build();

            QueueManager app = (QueueManager)bld.Services.GetService(typeof(QueueManager));
            Task.Run(() => app.Run()).Wait();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(configure => configure.ValidateScopes = false)
                .UseCloudFoundryHosting() //Enable listening on a Env provided port
			    .AddConfigServer()
                .UseStartup<Startup>();
            builder.ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddDynamicConsole();
            });
            return builder;
        }
    }
}
