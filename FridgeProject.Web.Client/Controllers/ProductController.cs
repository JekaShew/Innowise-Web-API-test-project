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
    public class ProductController : BaseController
    {
        private readonly IProduct productService;

        public ProductController(IProduct productService)
        {
            this.productService = productService;
        }

        [HttpGet("TakeAll")]
        public async Task<ActionResult> TakeAll()
        {
            try
            {
                var result = await productService.TakeProducts();
                return View(result);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("TakeById/{id}")]
        public async Task<ActionResult> TakeById([FromRoute]Guid id)
        {
            try
            {
                var result = await productService.TakeProductById(id);
                return View(result);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }

        }

        [HttpGet("Add")]
        public ActionResult Add()
        {
            return View(new Product());
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    await productService.AddProduct(product);
                    return RedirectToAction(nameof(TakeAll));
                }
                else
                    return View(product);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
            
        }


        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            try
            {
                var product = await productService.TakeProductById(id);
                await productService.DeleteProduct(product);
                return RedirectToAction(nameof(TakeAll));
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id)
        {
            return View(await productService.TakeProductById(id));
        }
        [HttpPost("Update")]
        public async Task<ActionResult> Update(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await productService.UpdateProduct(product);
                    return RedirectToAction(nameof(TakeAll));
                }
                else
                    return View(product);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }
    }
}