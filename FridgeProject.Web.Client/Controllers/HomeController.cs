using FridgeProject.Abstract;
using FridgeProject.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FridgeProject.Web.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFridgeServices _fridgeService;
        public HomeController(ILogger<HomeController> logger, IFridgeServices fridgeService )
        {
            _logger = logger;
            _fridgeService = fridgeService;
        }

        public IActionResult Index()
        {        
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
