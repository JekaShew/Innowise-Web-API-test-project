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
        [HttpGet("takebyid/{id}")]
        public async Task<IActionResult> GetFridgeModelById(Guid id)
        {
            var result = await fridgeServices.TakeFridgeById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("takeall")]
        public async Task<IActionResult> GetFridges( )
        {
            var result = await fridgeServices.TakeFridges();          
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFridge([FromBody] Fridge fridge)
        {
            await fridgeServices.DeleteFridge(fridge);
            return Ok();               
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddFridge([FromBody] Fridge fridge)
        {
            await fridgeServices.AddFridge(fridge);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateFridge([FromBody]Fridge fridge)
        {
            await fridgeServices.UpdateFridge(fridge);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updatefridgeproductswithoutquantity")]
        public async Task<IActionResult> UpdateFridgeProductsWithoutQuantity()
        {  
            await fridgeServices.UpdateFridgeProductsWithoutQuantity();
            return Ok();  
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("takeandupdatefridgeswithoutquantity")]
        public async Task<IActionResult> GetUpdatedFridgeProductsWothoutQuantity()
        {
            var updatedFridgesWithoutQuantity = await fridgeServices.TakeUpdatedFridgesWithoutQuantity();
            if (updatedFridgesWithoutQuantity != null)
                return Ok(updatedFridgesWithoutQuantity);
            else
                return NotFound();
        }   
    }
}
