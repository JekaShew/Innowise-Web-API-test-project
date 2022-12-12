using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Web.Client.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Services
{
    public class AccountServices : IAccountServices, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly RemoteConfig _remoteConfig;
        public AccountServices(IOptions<RemoteConfig> options)
        {
            _httpClient = new HttpClient();
            _remoteConfig = options.Value;
        }
        public async Task<AuthorizationInfo> LogIn(LogInInfo logIn)
        {          
                var response = await _httpClient.PostAsync
                 (
                     $"{_remoteConfig.BaseUrl}/api/account/login",
                     new StringContent(JsonConvert.SerializeObject(logIn), System.Text.Encoding.UTF8, "application/json")
                 );
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<AuthorizationInfo>(await response.Content.ReadAsStringAsync());
        }
           
        public async Task LogOut()
        {
            await _httpClient.PostAsync
             (
                $"{_remoteConfig.BaseUrl}/api/account/logout",
                   new StringContent("")
            ); 
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

    public static class AccountServicesExtensions
    {
        public static IServiceCollection AddAccountServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountServices, AccountServices>();
            return services;
        }
    }
}
