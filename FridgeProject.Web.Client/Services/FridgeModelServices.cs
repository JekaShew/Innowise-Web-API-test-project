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
    public class FridgeModelServices :  BaseClientService,IFridgeModel, IDisposable
    {
        public FridgeModelServices(IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor) :base(options,httpContextAccessor)
        {
           
        }

        public async Task AddFridgeModel(FridgeModel fridgeModel)
        {
            var response = await SendRequest(HttpMethod.Post, "fridgemodels", "addfridgemodel", SerializeInJson(fridgeModel));
            response.EnsureSuccessStatusCode();  
        }

        public async Task DeleteFridgeModel(FridgeModel fridgeModel)
        {
            var response = await SendRequest(HttpMethod.Delete, "fridgemodels", "deletefridgemodel", SerializeInJson(fridgeModel));
            response.EnsureSuccessStatusCode();
        }

        public async Task<FridgeModel> GetFridgeModelById(Guid id)
        {
            var response = await SendRequest(HttpMethod.Get, "fridgemodels", $"getfridgemodelbyid/{id}", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<FridgeModel>(response);
        }

        public async Task<List<FridgeModel>> GetFridgeModels()
        {
            var response = await SendRequest(HttpMethod.Get, "fridgemodels", "getfridgemodels", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<List<FridgeModel>>(response);
        }

        public async Task UpdateFridgeModel(FridgeModel updatedFridgeModel)
        {
            var response = await SendRequest(HttpMethod.Put, "fridgemodels", "updatefridgemodel", SerializeInJson(updatedFridgeModel));
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
