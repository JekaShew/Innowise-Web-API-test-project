using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Web.Client.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Services
{
    public class AccountServices : IAccount, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly RemoteConfig remoteConfig;
        public AccountServices(IOptions<RemoteConfig> options)
        {
            httpClient = new HttpClient();
            remoteConfig = options.Value;
        }
        public async Task<AuthorizationInfo> LogIn(LogInInfo logIn)
        {
           
                var response = await httpClient.PostAsync
                 (
                     $"{remoteConfig.BaseUrl}/api/account/login",
                     new StringContent(JsonConvert.SerializeObject(logIn), System.Text.Encoding.UTF8, "application/json")
                 );
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<AuthorizationInfo>(await response.Content.ReadAsStringAsync());
        }
           
        public async Task LogOut()
        {
            await httpClient.PostAsync
             (
                $"{remoteConfig.BaseUrl}/api/account/logout",
                   new StringContent("")
            ); 
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
