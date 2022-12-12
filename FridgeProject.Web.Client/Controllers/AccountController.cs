using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Controllers
{  
    [Route("/Account")] 
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountServices;
   
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        [HttpGet("Login")]
        public IActionResult Login()
        {
            ViewBag.UserNotFound = "";
            return View("Login", new LogInInfo());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login( LogInInfo logInInfo)
        {
            try
            {
                var authorizationInfo = await _accountServices.LogIn(logInInfo);
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
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AUTHORIZATION_BEARER");
            return RedirectToAction(nameof(Login));
        }
    }
}
