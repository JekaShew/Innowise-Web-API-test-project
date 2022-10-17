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
    [Route("/api/fridges")]
    public class FridgeController : Controller
    {
        private readonly IFridge fridgeServices;

        public FridgeController(IFridge fridgeServices)
        {
            this.fridgeServices = fridgeServices;
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("getfridgebyid/{id}")]
        public async Task<IActionResult> GetFridgeModelById(Guid id)
        {
            var result = await fridgeServices.GetFridgeById(id);
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            else
                return Unauthorized();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("getfridges")]
        public async Task<IActionResult> GetFridges( )
        {
            var result = await fridgeServices.GetFridges();
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            else
            return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deletefridge")]
        public async Task<IActionResult> DeleteFridge([FromBody] Fridge fridge)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true) 
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await fridgeServices.DeleteFridge(fridge);
                    return Ok();
                }
                else return Forbid();
            }         
            else return Unauthorized();                   
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addfridge")]
        public async Task<IActionResult> AddFridge([FromBody] Fridge fridge)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await fridgeServices.AddFridge(fridge);
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updatefridge")]
        public async Task<IActionResult> UpdateFridge([FromBody]Fridge fridge)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await fridgeServices.UpdateFridge(fridge);
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("updatefridgeproductswithoutquantity")]
        public async Task<IActionResult> UpdateFridgeProductsWithoutQuantity()
        {  
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await fridgeServices.UpdateFridgeProductsWithoutQuantity();
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("getandupdatefridgeswithoutquantity")]
        public async Task<IActionResult> GetUpdatedFridgeProductsWothoutQuantity()
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    var updatedFridgesWithoutQuantity = await fridgeServices.GetUpdatedFridgesWithoutQuantity();
                    if (updatedFridgesWithoutQuantity != null)
                        return Ok(updatedFridgesWithoutQuantity);
                    else
                        return NotFound();
                    
                }
                else return Forbid();
            }
            else return Unauthorized();  
        }   
    }
}
