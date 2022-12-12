using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Controllers
{

    [Route("/Product")]
    public class ProductController : BaseController
    {
        private readonly IProductServices _productService;

        public ProductController(IProductServices productService)
        {
           _productService = productService;
        }

        [HttpGet("TakeAll")]
        public async Task<ActionResult> TakeAll()
        {
            try
            {
                var result = await _productService.TakeProducts();
                return View(result);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> TakeById([FromRoute]Guid id)
        {
            try
            {
                var result = await _productService.TakeProductById(id);
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

                    await _productService.AddProduct(product);
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
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            { 
                await _productService.DeleteProduct(id);
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
            return View(await _productService.TakeProductById(id));
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.UpdateProduct(product);
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