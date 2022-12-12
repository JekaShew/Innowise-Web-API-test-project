using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Web.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Services
{
    public class ProductServices : BaseClientService,IProductServices, IDisposable
    {
        public ProductServices(IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor) { }

        public async Task AddProduct(Product product)
        {
            var response = await SendRequest(HttpMethod.Post, "products", "", SerializeInJson(product));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Guid id)
        {
            var response = await SendRequest(HttpMethod.Delete, "products", "", SerializeInJson(id));
            response.EnsureSuccessStatusCode();
        }

        public async Task<Product> TakeProductById(Guid id)
        {
            var response = await SendRequest(HttpMethod.Get, "products", $"{id}", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<Product>(response);
        }

        public async Task<List<Product>> TakeProducts()
        {
            var response = await SendRequest(HttpMethod.Get, "products", "", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<List<Product>>(response);
        }

        public async Task UpdateProduct(Product updatedProdcut)
        {
            var response = await SendRequest(HttpMethod.Put, "products", "", SerializeInJson(updatedProdcut));
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }

    public static class ProductServicesExtensions
    {
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            services.AddTransient<IProductServices, ProductServices>();
            return services;
        }
    }
}

