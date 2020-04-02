using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using WingtipToys.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Discovery.Client;
using WingtipToys.Client;
using Microsoft.AspNetCore.Http;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.CircuitBreaker.Hystrix;

namespace WingtipToys
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCloudFoundryActuators(Configuration);

            // Load Product Service Options 
            services.Configure<WingtipToysProductServiceOptions>(Configuration.GetSection("WingtipToys-ProductService"));
            // End load Fortune Service

            // Add service client library for calling the Product Service
            services.AddScoped<IProductService, WingtipToysProductServiceClient>();
            // End add service client

            // Add management components which collect and forwards metrics to 
            // the Cloud Foundry Metrics Forwarder service
            // Remove comments below to enable
            services.AddMetricsActuator(Configuration);
            services.AddMetricsForwarderExporter(Configuration);

            // Add Session Caching function
            services.AddDistributedMemoryCache();
            services.AddSession();
            // End Session Cache

            // Add Circuit Breaker function
            services.AddHystrixCommand<GetAllProductsCommand>("GetAllProductsService", Configuration);
            services.AddHystrixCommand<GetProductsCommand>("WingtipToys-ProductService", Configuration);
            services.AddHystrixCommand<GetProductCommand>("WingtipToys-ProductService", Configuration);
            services.AddHystrixMetricsStream(Configuration);
            // End Add CB

            try
            {
                services.AddDiscoveryClient(Configuration);
            }
            catch { }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCloudFoundryActuators();

            try
            {
                app.UseDiscoveryClient();
            }
            catch
            { }

            // Use Hystrix
            app.UseHystrixRequestContext();

            // Use Session Cache
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Add metrics collection to the app
            // Remove comment below to enable
            app.UseMetricsActuator();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
