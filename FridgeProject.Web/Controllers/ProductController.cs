using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Web.Controllers
{

    [Route("/api/products")]
    public class ProductController : Controller
    {
        private readonly IProduct productServices;

        public ProductController(IProduct productServices)
        {
            this.productServices = productServices;

        }

        [Authorize(Roles ="Client,Admin")]
        [HttpGet("takebyid/{id}")]
        public async Task<IActionResult> TakeProductById(Guid id)
        {
            var result = await productServices.TakeProductById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("takeall")]
        public async Task<IActionResult> TakeProducts()
        {
            var result = await productServices.TakeProducts();   
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct([FromBody] Product product)
        {
            await productServices.DeleteProduct(product);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await productServices.AddProduct(product);
            return Ok();  
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            await productServices.UpdateProduct(product);
            return Ok();
        }
    }
}
