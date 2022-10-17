using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Controllers
{  
    [Route("/Account")] 
    public class AccountController : Controller
    {
        private readonly IAccount accountServices;
   

        public AccountController(IAccount accountServices)
        {
            this.accountServices = accountServices;

        }
        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {
            ViewBag.UserNotFound = "";
            return View(new LogInInfo());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login( LogInInfo logInInfo)
        {
            try
            {
                var authorizationInfo = await accountServices.LogIn(logInInfo);
                Response.Cookies.Append("AUTHORIZATION_BEARER", authorizationInfo.Token);
                return RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException)
            {
                ViewBag.UserNotFound = "User not Found";
                return View(logInInfo);
            }


            
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await accountServices.LogOut();
            Response.Cookies.Delete("AUTHORIZATION_BEARER");
            return RedirectToAction(nameof(Login));
        }
    }
}
