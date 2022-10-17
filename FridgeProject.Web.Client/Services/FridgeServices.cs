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
    public class FridgeServices : IFridge
    {
        private readonly HttpClient httpClient;
        private readonly RemoteConfig remoteConfig;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FridgeServices(IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor)
        {
            httpClient = new HttpClient();
            remoteConfig = options.Value;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task AddFridge(Fridge fridge)
        {          
            var request = new HttpRequestMessage(HttpMethod.Post, $"{remoteConfig.BaseUrl}/api/fridges/addfridge");
            request.Content = new StringContent(JsonConvert.SerializeObject(fridge, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFridge(Fridge fridge)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{remoteConfig.BaseUrl}/api/fridges/deletefridge");
            request.Content = new StringContent(JsonConvert.SerializeObject(fridge, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("AUTHORIZATION", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Fridge> GetFridgeById(Guid id)
        {
            
            var request = new HttpRequestMessage(HttpMethod.Get, $"{remoteConfig.BaseUrl}/api/fridges/getfridgebyid/{id}");
            request.Headers.Add("AUTHORIZATION", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Fridge>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });  
        }

        public async Task<List<Fridge>> GetFridges()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{remoteConfig.BaseUrl}/api/fridges/getfridges");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Fridge>>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task UpdateFridge(Fridge updatedFridge)
        {        
            var request = new HttpRequestMessage(HttpMethod.Put, $"{remoteConfig.BaseUrl}/api/fridges/updatefridge");
            request.Content = new StringContent(JsonConvert.SerializeObject(updatedFridge), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("AUTHORIZATION", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }     

        public async Task UpdateFridgeProductsWithoutQuantity()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{remoteConfig.BaseUrl}/api/fridges/updatefridgeproductswithoutquantity");
            request.Headers.Add("AUTHORIZATION", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Fridge>> GetUpdatedFridgesWithoutQuantity()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{remoteConfig.BaseUrl}/api/fridges/getandupdatefridgeswithoutquantity");
            request.Content = new StringContent("");
            request.Headers.Add("AUTHORIZATION", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Fridge>>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
