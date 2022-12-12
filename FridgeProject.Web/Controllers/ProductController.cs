using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FridgeProject.Web.Controllers
{

    [Route("/api/products")]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [Authorize(Roles ="Client,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> TakeProductById(Guid id)
        {
            var result = await _productServices.TakeProductById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet]
        public async Task<IActionResult> TakeProducts()
        {
            var result = await _productServices.TakeProducts();   
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromBody] Guid id)
        {
            await _productServices.DeleteProduct(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _productServices.AddProduct(product);
            return Ok();  
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            await _productServices.UpdateProduct(product);
            return Ok();
        }
    }
}
