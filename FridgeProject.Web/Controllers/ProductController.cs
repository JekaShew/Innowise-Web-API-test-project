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
        [HttpGet("getproductbyid/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await productServices.GetProductById(id);
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            else
                return Unauthorized();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("getproducts")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await productServices.GetProducts();
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            else
                return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteproduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] Product product)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await productServices.DeleteProduct(product);
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await productServices.AddProduct(product);
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await productServices.UpdateProduct(product);
                    return Ok();

                }
                else return Forbid();
            }
            else return Unauthorized();
        }
    }
}
