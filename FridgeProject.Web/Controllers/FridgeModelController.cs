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
    [Route("/api/fridgemodels")]
    public class FridgeModelController : Controller
    {
        private readonly IFridgeModel fridgeModelServices;

        public FridgeModelController(IFridgeModel fridgeServices)
        {
            this.fridgeModelServices = fridgeServices;
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("takebyid/{id}")]
        public async Task<IActionResult> TakeFridgeModelById(Guid id)
        {
            var result = await fridgeModelServices.TakeFridgeModelById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("takeall")]
        public async Task<IActionResult> TakeFridgeModels()
        {
            var result = await fridgeModelServices.TakeFridgeModels();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFridgeModel([FromBody] FridgeModel fridgeModel)
        {
            await fridgeModelServices.DeleteFridgeModel(fridgeModel);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddFridgeModel([FromBody] FridgeModel fridgeModel)
        {
            await fridgeModelServices.AddFridgeModel(fridgeModel);
            return Ok();   
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateFridgeMoadel([FromBody] FridgeModel fridgeModel)
        {
            await fridgeModelServices.UpdateFridgeModel(fridgeModel);
            return Ok();
        }
    }
}
