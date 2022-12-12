using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FridgeProject.Web.Controllers
{
    [Route("/api/fridgemodels")]
    public class FridgeModelController : Controller
    {
        private readonly IFridgeModelServices _fridgeModelServices;

        public FridgeModelController(IFridgeModelServices fridgeServices)
        {
            _fridgeModelServices = fridgeServices;
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> TakeFridgeModelById(Guid id)
        {
            var result = await _fridgeModelServices.TakeFridgeModelById(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet]
        public async Task<IActionResult> TakeFridgeModels()
        {
            var result = await _fridgeModelServices.TakeFridgeModels();
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFridgeModel([FromBody] Guid id)
        {
            await _fridgeModelServices.DeleteFridgeModel(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFridgeModel([FromBody] FridgeModel fridgeModel)
        {
            await _fridgeModelServices.AddFridgeModel(fridgeModel);
            return Ok();   
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateFridgeMoadel([FromBody] FridgeModel fridgeModel)
        {
            await _fridgeModelServices.UpdateFridgeModel(fridgeModel);
            return Ok();
        }
    }
}
