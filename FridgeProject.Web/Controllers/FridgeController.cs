using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FridgeProject.Web.Controllers
{
    [Route("/api/fridges")]
    public class FridgeController : Controller
    {
        private readonly IFridgeServices _fridgeServices;

        public FridgeController(IFridgeServices fridgeServices)
        {
            _fridgeServices = fridgeServices;
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFridgeModelById(Guid id)
        {
            var result = await _fridgeServices.TakeFridgeById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetFridges( )
        {
            var result = await _fridgeServices.TakeFridges();          
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFridge([FromBody] Guid id)
        {
            await _fridgeServices.DeleteFridge(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFridge([FromBody] Fridge fridge)
        {
            await _fridgeServices.AddFridge(fridge);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateFridge([FromBody]Fridge fridge)
        {
            await _fridgeServices.UpdateFridge(fridge);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updat-fridges-without-quantity")]
        public async Task<IActionResult> UpdateFridgeProductsWithoutQuantity()
        {  
            await _fridgeServices.UpdateFridgeProductsWithoutQuantity();
            return Ok();  
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-and-take-fridges-without-quantity")]
        public async Task<IActionResult> TakeUpdatedFridgeProductsWothoutQuantity()
        {
            var updatedFridgesWithoutQuantity = await _fridgeServices.TakeUpdatedFridgesWithoutQuantity();
            if (updatedFridgesWithoutQuantity != null)
                return Ok(updatedFridgesWithoutQuantity);
            else
                return NotFound();
        }   
    }
}
