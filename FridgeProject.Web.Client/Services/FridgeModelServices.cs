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
    public class FridgeModelServices :  BaseClientService,IFridgeModelServices, IDisposable
    {
        public FridgeModelServices(IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor) { }

        public async Task AddFridgeModel(FridgeModel fridgeModel)
        {
            var response = await SendRequest(HttpMethod.Post, "fridgemodels", "", SerializeInJson(fridgeModel));
            response.EnsureSuccessStatusCode();  
        }

        public async Task DeleteFridgeModel(Guid id)
        {
            var response = await SendRequest(HttpMethod.Delete, "fridgemodels", "", SerializeInJson(id));
            response.EnsureSuccessStatusCode();
        }

        public async Task<FridgeModel> TakeFridgeModelById(Guid id)
        {
            var response = await SendRequest(HttpMethod.Get, "fridgemodels", $"{id}", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<FridgeModel>(response);
        }

        public async Task<List<FridgeModel>> TakeFridgeModels()
        {
            var response = await SendRequest(HttpMethod.Get, "fridgemodels", "", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<List<FridgeModel>>(response);
        }

        public async Task UpdateFridgeModel(FridgeModel updatedFridgeModel)
        {
            var response = await SendRequest(HttpMethod.Put, "fridgemodels", "", SerializeInJson(updatedFridgeModel));
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }

    public static class FridgeModelServicesExtensions
    {
        public static IServiceCollection AddFridgeModelServices(this IServiceCollection services)
        {
            services.AddTransient<IFridgeModelServices, FridgeModelServices>();
            return services;
        }
    }
}
