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
        [HttpGet("getfridgemodelbyid/{id}")]
        public async Task<IActionResult> GetFridgeModelById(Guid id)
        {
            var result = await fridgeModelServices.GetFridgeModelById(id);
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            else
                return Unauthorized();
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet("getfridgemodels")]
        public async Task<IActionResult> GetFridgeModels()
        {
            var result = await fridgeModelServices.GetFridgeModels();
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
                if (result != null)
                    return Ok(result);
                else
                    return NotFound();
            else
                return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deletefridgemodel")]
        public async Task<IActionResult> DeleteFridgeModel([FromBody] FridgeModel fridgeModel)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await fridgeModelServices.DeleteFridgeModel(fridgeModel);
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addfridgemodel")]
        public async Task<IActionResult> AddFridgeModel([FromBody] FridgeModel fridgeModel)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await fridgeModelServices.AddFridgeModel(fridgeModel);
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updatefridgemodel")]
        public async Task<IActionResult> UpdateFridgeMoadel([FromBody] FridgeModel fridgeModel)
        {
            if (this.HttpContext.User.Identity.IsAuthenticated == true)
            {
                if (this.HttpContext.User.IsInRole("Admin"))
                {
                    await fridgeModelServices.UpdateFridgeModel(fridgeModel);
                    return Ok();
                }
                else return Forbid();
            }
            else return Unauthorized();
        }
    }
}
