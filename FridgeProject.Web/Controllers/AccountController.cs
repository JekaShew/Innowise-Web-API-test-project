using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Web.Controllers
{
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountServices;

        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody]LogInInfo logIn)
        {
            if (ModelState.IsValid)
            { 
                    var user = await _accountServices.LogIn(logIn);
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
    }
}
