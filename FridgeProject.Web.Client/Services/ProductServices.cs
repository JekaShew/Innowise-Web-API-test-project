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
    public class ProductServices : BaseClientService,IProduct, IDisposable
    {
        public ProductServices( IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor) : base(options,httpContextAccessor)
        {
           
        }

        public async Task AddProduct(Product product)
        {
            var response = await SendRequest(HttpMethod.Post, "products", "addproduct", SerializeInJson(product));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Product product)
        { 
            var response = await SendRequest(HttpMethod.Delete, "products", "deleteproduct", SerializeInJson(product));
            response.EnsureSuccessStatusCode();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var response = await SendRequest(HttpMethod.Get, "products", $"getproductbyid/{id}", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<Product>(response);
        }

        public async Task<List<Product>> GetProducts()
        {
            var response = await SendRequest(HttpMethod.Get, "products", "getproducts", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<List<Product>>(response);
        }

        public async Task UpdateProduct(Product updatedProdcut)
        {
            var response = await SendRequest(HttpMethod.Put, "products", "updateproduct", SerializeInJson(updatedProdcut));
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    } 
}

