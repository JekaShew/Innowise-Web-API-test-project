using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace FridgeProject.Web.Client.Controllers
{
    public abstract class BaseController : Controller
    {
        protected  ActionResult CatchHttpRequestExeption(HttpRequestException httpRequestException)
        {
            if (httpRequestException.StatusCode == System.Net.HttpStatusCode.NotFound)
                return View("~/Views/Errors/NotFound.cshtml");
            if (httpRequestException.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (httpRequestException.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return View("~/Views/Errors/AccessDenied.cshtml");
            return View("~/Views/Errors/CommonError.cshtml");
        }
    }
}
