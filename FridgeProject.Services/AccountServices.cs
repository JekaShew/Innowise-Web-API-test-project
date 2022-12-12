using AutoMapper;
using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly AppDBContext _appDBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityInfo _identityInfo;
        private readonly IMapper _mapper;

        public AccountServices(AppDBContext appDBContext, IHttpContextAccessor httpContextAccessor, IOptions<IdentityInfo> identityInfo, IMapper mapper)
        {
            _appDBContext = appDBContext;
            _httpContextAccessor = httpContextAccessor;
            _identityInfo = identityInfo.Value;
            _mapper = mapper;
        }

        public async Task<AuthorizationInfo> LogIn(LogInInfo logIn)
        {
            var userDB = await _appDBContext.Users.FirstOrDefaultAsync(u => u.Login == logIn.Login && u.Password == logIn.Password);
            if (userDB == null)
                return null;
            var user = _mapper.Map<User>(userDB);
            var identity = BuildIdentity(user);  
            return new AuthorizationInfo
            {
                Token = BuildJWTToken(identity),
                Name = user.Login,
                Role = user.Role
            };
        }  

        private ClaimsIdentity BuildIdentity(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return new ClaimsIdentity(claims, ClaimsIdentity.DefaultRoleClaimType);
        }

        private string BuildJWTToken(ClaimsIdentity identity)
        {
            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                    issuer: _identityInfo.Issuer,
                    audience: _identityInfo.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_identityInfo.LifeTimeInMSeconds)),
                    signingCredentials: new SigningCredentials
                        (
                        new SymmetricSecurityKey
                            (
                                Encoding.UTF8.GetBytes(_identityInfo.Key)
                            ),
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }

    public static class AccountServicesExtensions
    {
        public static IServiceCollection AddAccountServices(this IServiceCollection services, IdentityInfo identityInfo)
        {
            services.Configure<IdentityInfo>(options =>
            {
                options.Audience = identityInfo.Audience;
                options.Issuer = identityInfo.Issuer;
                options.LifeTimeInMSeconds = identityInfo.LifeTimeInMSeconds;
                options.Key = identityInfo.Key;
            });
            services.AddTransient<IAccountServices, AccountServices>();
            return services;
        }     
    }
}
