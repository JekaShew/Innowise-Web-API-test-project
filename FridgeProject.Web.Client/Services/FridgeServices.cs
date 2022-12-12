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
    public class FridgeServices : BaseClientService,IFridgeServices, IDisposable
    {

        public FridgeServices(IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor) { }

        public async Task AddFridge(Fridge fridge)
        {
            var response = await SendRequest(HttpMethod.Post, "fridges", "", SerializeInJson(fridge));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFridge(Guid id)
        {
            var response = await SendRequest(HttpMethod.Delete, "fridges", "", SerializeInJson(id));
            response.EnsureSuccessStatusCode();
        }

        public async Task<Fridge> TakeFridgeById(Guid id)
        {
            var response = await SendRequest(HttpMethod.Get, "fridges", $"{id}", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<Fridge>(response);
        }

        public async Task<List<Fridge>> TakeFridges()
        {
            var response = await SendRequest(HttpMethod.Get, "fridges", "", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<List<Fridge>>(response);
        }

        public async Task UpdateFridge(Fridge updatedFridge)
        {
            var response = await SendRequest(HttpMethod.Put, "fridges", "", SerializeInJson(updatedFridge));
            response.EnsureSuccessStatusCode();
        }     

        public async Task UpdateFridgeProductsWithoutQuantity()
        {
            var response = await SendRequest(HttpMethod.Post, "fridges", "update-fridges-quantity", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Fridge>> TakeUpdatedFridgesWithoutQuantity()
        {
            var response = await SendRequest(HttpMethod.Put, "fridges", "update-and-take-fridges-without-quantity", new StringContent(""));
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<List<Fridge>>(response);
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }

    public static class FridgeServicesExtensions
    {
        public static IServiceCollection AddFridgeServices(this IServiceCollection services)
        {
            services.AddTransient<IFridgeServices, FridgeServices>();
            return services;
        }
    }
}
