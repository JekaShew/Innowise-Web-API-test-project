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
    public class FridgeModelServices : IFridgeModel, IDisposable
    {

        private readonly HttpClient httpClient;
        private readonly RemoteConfig remoteConfig;
        private readonly IHttpContextAccessor httpContextAccessor;
        public FridgeModelServices( IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor)
        {
            httpClient = new HttpClient();
            remoteConfig = options.Value;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task AddFridgeModel(FridgeModel fridgeModel)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{remoteConfig.BaseUrl}/api/fridgemodels/addfridgemodel");
            request.Content = new StringContent(JsonConvert.SerializeObject(fridgeModel, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteFridgeModel(FridgeModel fridgeModel)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{remoteConfig.BaseUrl}/api/fridgemodels/deletefridgemodel");
            request.Content = new StringContent(JsonConvert.SerializeObject(fridgeModel, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<FridgeModel> GetFridgeModelById(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{remoteConfig.BaseUrl}/api/fridgemodels/getfridgemodelbyid/{id}");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject< FridgeModel>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task<List<FridgeModel>> GetFridgeModels()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{remoteConfig.BaseUrl}/api/fridgemodels/getfridgemodels");
            request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<FridgeModel>>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public async Task UpdateFridgeModel(FridgeModel updatedFridgeModel)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{remoteConfig.BaseUrl}/api/fridgemodels/updatefridgemodel");
            request.Content = new StringContent(JsonConvert.SerializeObject(updatedFridgeModel, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), System.Text.Encoding.UTF8, "application/json");
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
