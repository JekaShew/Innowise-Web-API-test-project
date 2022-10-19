using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Controllers
{
    public abstract class BaseController : Controller
    {
        protected  ActionResult CatchHttpRequestExeption(HttpRequestException httpRequestException)
        {
            if ((int)httpRequestException.StatusCode == 404)
                return View("~/Views/Errors/NotFound.cshtml");
            if ((int)httpRequestException.StatusCode == 401)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if ((int)httpRequestException.StatusCode == 403)
                return View("~/Views/Errors/AccessDenied.cshtml");
            return View("~/Views/Errors/CommonError.cshtml");
        }
    }
}
