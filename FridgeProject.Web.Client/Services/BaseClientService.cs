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
    public class BaseClientService
    {
        protected readonly HttpClient httpClient;
        protected readonly RemoteConfig remoteConfig;
        protected readonly IHttpContextAccessor httpContextAccessor;
        public BaseClientService(IOptions<RemoteConfig> options, IHttpContextAccessor httpContextAccessor)
        {
            httpClient = new HttpClient();
            remoteConfig = options.Value;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<HttpResponseMessage> SendRequest(HttpMethod methodType, string querryServicesGroup, string querrySelectedService, StringContent stringContent)
        {
            var request = new HttpRequestMessage(methodType, $"{remoteConfig.BaseUrl}/api/{querryServicesGroup}/{querrySelectedService}");

            if (request.RequestUri != null)
            {
                if (stringContent != null)
                {
                    request.Content = stringContent;
                }
                request.Headers.Add("Authorization", $"Bearer {httpContextAccessor.HttpContext.Request.Cookies["AUTHORIZATION_BEARER"]}");
                return await httpClient.SendAsync(request);
 
            }
            else return null;
        }

        public StringContent SerializeInJson(object model)
        {
         
            return new StringContent(JsonConvert.SerializeObject
                    (
                        model,
                        new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        System.Text.Encoding.UTF8, "application/json"
                    );
        }

        public async Task<T> DeSerializeJson<T>(HttpResponseMessage responseMessage)
        {
            
            return  JsonConvert.DeserializeObject<T>
                (
                    await responseMessage.Content.ReadAsStringAsync(),
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
                );
        }
    }
}
