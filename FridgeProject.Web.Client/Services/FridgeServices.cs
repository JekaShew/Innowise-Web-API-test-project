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
    public class FridgeServices : BaseClientService,IFridge, IDisposable
    {
        
        public FridgeServices(IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor) : base(options,httpContextAccessor)
        {
            
        }
        public async Task AddFridge(Fridge fridge)
        {
            var response = await SendRequest(HttpMethod.Post, "fridges", "addfridge", SerializeInJson(fridge));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFridge(Fridge fridge)
        {
            var response = await SendRequest(HttpMethod.Delete, "fridges", "deletefridge", SerializeInJson(fridge));
            response.EnsureSuccessStatusCode();
        }

        public async Task<Fridge> GetFridgeById(Guid id)
        {
            var response = await SendRequest(HttpMethod.Get, "fridges", $"getfridgebyid/{id}", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<Fridge>(response);
        }

        public async Task<List<Fridge>> GetFridges()
        {
            var response = await SendRequest(HttpMethod.Get, "fridges", "getfridges", null);
            response.EnsureSuccessStatusCode();
            return await DeSerializeJson<List<Fridge>>(response);
        }

        public async Task UpdateFridge(Fridge updatedFridge)
        {
            var response = await SendRequest(HttpMethod.Put, "fridges", "updatefridge", SerializeInJson(updatedFridge));
            response.EnsureSuccessStatusCode();
        }     

        public async Task UpdateFridgeProductsWithoutQuantity()
        {
            var response = await SendRequest(HttpMethod.Post, "fridges", "updatefridgeproductswithoutquantity", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Fridge>> GetUpdatedFridgesWithoutQuantity()
        {
            var response = await SendRequest(HttpMethod.Put, "fridges", "getandupdatefridgeswithoutquantity", new StringContent(""));
            response.EnsureSuccessStatusCode();
            
            return await DeSerializeJson<List<Fridge>>(response);
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
