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
   
    [Route("/Product")]
    public class ProductController : Controller
    {
        private readonly IProduct productService;

        public ProductController(IProduct productService)
        {
            this.productService = productService;
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                var result = await productService.GetProducts();
                return View(result);
            }
            catch (HttpRequestException e)
            {
                if ((int)e.StatusCode == 404)
                    return View("~/Views/Errors/NotFound.cshtml");
                if ((int)e.StatusCode == 401)
                    return View("~/Views/Errors/Unauthorized.cshtml");
                if ((int)e.StatusCode == 403)
                    return View("~/Views/Errors/AccessDenied.cshtml");
                return View();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductById(Guid id)
        {
            try
            {
                var result = await productService.GetProductById(id);
                if (result != null)
                {
                    return View(result);
                }
                else
                    return View();
            }
            catch (HttpRequestException e)
            {
                if ((int)e.StatusCode == 404)
                    return View("~/Views/Errors/NotFound.cshtml");
                if ((int)e.StatusCode == 401)
                    return View("~/Views/Errors/Unauthorized.cshtml");
                if ((int)e.StatusCode == 403)
                    return View("~/Views/Errors/AccessDenied.cshtml");
                return View();
            }

        }

        [HttpGet("Add")]
        public ActionResult AddProduct()
        {
            return View(new Product());
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await productService.AddProduct(product);
                    return RedirectToAction(nameof(GetProducts));
                }
                catch (HttpRequestException e)
                {
                    if ((int)e.StatusCode == 404)
                        return View("~/Views/Errors/NotFound.cshtml");
                    if ((int)e.StatusCode == 401)
                        return View("~/Views/Errors/Unauthorized.cshtml");
                    if ((int)e.StatusCode == 403)
                        return View("~/Views/Errors/AccessDenied.cshtml");
                    return View();
                }
            }
            else
                return View(product);
        }


        [HttpGet("Delete")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var product = await productService.GetProductById(id);
                if (product != null)
                {
                    await productService.DeleteProduct(product);
                    return RedirectToAction(nameof(GetProducts));
                }
                else
                    return View();
            }
            catch (HttpRequestException e)
            {
                if ((int)e.StatusCode == 404)
                    return View("~/Views/Errors/NotFound.cshtml");
                if ((int)e.StatusCode == 401)
                    return View("~/Views/Errors/Unauthorized.cshtml");
                if ((int)e.StatusCode == 403)
                    return View("~/Views/Errors/AccessDenied.cshtml");
                return View();
            }
        }

        [HttpGet("Update")]
        public async Task<ActionResult> UpdateProduct(Guid id)
        {
            return View(await productService.GetProductById(id));
        }
        [HttpPost("Update")]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await productService.UpdateProduct(product);
                    return RedirectToAction(nameof(GetProducts));
                }
                catch (HttpRequestException e)
                {
                    if ((int)e.StatusCode == 404)
                        return View("~/Views/Errors/NotFound.cshtml");
                    if ((int)e.StatusCode == 401)
                        return View("~/Views/Errors/Unauthorized.cshtml");
                    if ((int)e.StatusCode == 403)
                        return View("~/Views/Errors/AccessDenied.cshtml");
                    return View();
                }
            }
            else
                return View(product);
        }
    }
}