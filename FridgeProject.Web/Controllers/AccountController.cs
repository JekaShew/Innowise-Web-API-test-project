using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FridgeProject.Web.Controllers
{
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly IAccount accountServices;

        public AccountController(IAccount accountServices)
        {
            this.accountServices = accountServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody]LogInInfo logIn)
        {
            if (ModelState.IsValid)
            { 
                    var user = await accountServices.LogIn(logIn);
                if (user != null)
                    return Ok(user);
                else
                    return BadRequest(new { Error = "User not found!" });
            }
            else
            {
                return BadRequest(new { Error = ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {  
            await accountServices.LogOut();
            return Ok();
        }
    }
}
