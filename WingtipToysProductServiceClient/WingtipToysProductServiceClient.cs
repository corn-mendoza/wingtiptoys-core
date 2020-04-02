using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Steeltoe.Common.Discovery;
using WingtipToys.Models;

namespace WingtipToys.Client
{
    public class WingtipToysProductServiceClient : IProductService
    {

        ILogger<WingtipToysProductServiceClient> _logger;
        IOptionsSnapshot<WingtipToysProductServiceOptions> _config;

        private WingtipToysProductServiceOptions Config
        {
            get
            {
                return _config.Value;
            }
        }

        DiscoveryHttpClientHandler _handler;
        IHttpContextAccessor _reqContext;

        public WingtipToysProductServiceClient(
            IOptionsSnapshot<WingtipToysProductServiceOptions> config,
            ILogger<WingtipToysProductServiceClient> logger,

            IDiscoveryClient client,

            IHttpContextAccessor context = null)
        {
            _handler = new DiscoveryHttpClientHandler(client);

            _reqContext = context;

            _logger = logger;
            _config = config;

        }

        private async Task<T> HandleRequest<T>(string url) where T : class
        {
            _logger?.LogDebug("ProductService call: {url}", url);
            try
            {
                using (var client = await GetClientAsync())
                {
                    var stream = await client.GetStreamAsync(url);
                    var result = Deserialize<T>(stream);
                    _logger?.LogDebug("ProductService returned: {result}", result);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("ProductService exception: {0}", e);
                throw;
            }
        }

        private T Deserialize<T>(Stream stream) where T : class
        {
            try
            {
                using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (T)serializer.Deserialize(reader, typeof(T));
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("ProductService serialization exception: {0}", e);
            }
            return (T)null;
        }

        private async Task<HttpClient> GetClientAsync()
        {
            var client = new HttpClient(_handler, false);
            if (_reqContext != null)
            {
                var token = await _reqContext.HttpContext.GetTokenAsync("access_token");

                _logger?.LogDebug("GetClientAsync access token: {token}", token);

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            // Lab10 End

            return client;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await HandleRequest<List<Product>>(Config.GetAllProductsURL);
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await HandleRequest<Product>(Config.GetProductURL+"/"+id.ToString());
        }

        public async Task<List<Product>> GetProductsAsync(string name)
        {
            return await HandleRequest<List<Product>>(Config.GetProductsURL+"/"+name);
        }
    }
}