using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Web.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Services
{
    public class ProductServices : IProduct, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly RemoteConfig remoteConfig;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ProductServices( IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor)
        {
            httpClient = new HttpClient();
            remoteConfig = options.Value;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task AddProduct(Product product)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{remoteConfig.BaseUrl}/api/products/addproduct");
            request.Content = new StringContent(JsonConvert.SerializeObject(product, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Product product)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{remoteConfig.BaseUrl}/api/products/deleteproduct");
            request.Content = new StringContent(JsonConvert.SerializeObject(product, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{remoteConfig.BaseUrl}/api/products/getproductbyid/{id}");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task<List<Product>> GetProducts()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{remoteConfig.BaseUrl}/api/products/getproducts");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task UpdateProduct(Product updatedProdcut)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{remoteConfig.BaseUrl}/api/products/updateproduct");
            request.Content = new StringContent(JsonConvert.SerializeObject(updatedProdcut, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    } 
}

