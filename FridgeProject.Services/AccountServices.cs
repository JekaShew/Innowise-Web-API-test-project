using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Services
{
    public class AccountServices : IAccount
    {
        private readonly AppDBContext appDBContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AccountServices(AppDBContext appDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this.appDBContext = appDBContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthorizationInfo> LogIn(LogInInfo logIn)
        {
            var user = new User();
            var userDB = await appDBContext.Users.FirstOrDefaultAsync(u => u.Login == logIn.Login && u.Password == logIn.Password);

            if (userDB == null)
                return null;
            else
            {
                user.Id = userDB.Id;
                user.Login = userDB.Login;
                user.Role = userDB.Role;
                var identity = BuildIdentity(user);
                var now = DateTime.Now;
                var jwt = new JwtSecurityToken(
                        issuer: Constants.Identity.ISSUER,
                        audience: Constants.Identity.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(Constants.Identity.LIFETIME)),
                        signingCredentials: new SigningCredentials
                            (
                            new SymmetricSecurityKey
                                (
                                    Encoding.UTF8.GetBytes(Constants.Identity.KEY)
                                ),
                            SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new AuthorizationInfo
                {
                    Token = encodedJwt,
                    Name = user.Login,
                    Role = user.Role
                };
            }
        }
    

        public Task LogOut()
        {

            return Task.CompletedTask;
        } 
       
        private ClaimsIdentity BuildIdentity(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));

            return new ClaimsIdentity(claims, ClaimsIdentity.DefaultRoleClaimType);
        }

    }
}
