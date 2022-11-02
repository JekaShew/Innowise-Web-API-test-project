using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FridgeProject.Abstract.Data;
using System.Net.Http;
using Newtonsoft.Json;
using FridgeProject.Abstract;

namespace FridgeProject.Web.Client.Controllers
{
    [Route("/FridgeModel")]
    public class FridgeModelController : BaseController
    {
        private readonly IFridgeModel fridgeModelService;

        public FridgeModelController(IFridgeModel fridgeModelService)
        {
            this.fridgeModelService = fridgeModelService;
        }

        [HttpGet("TakeAll")]
        public async Task<ActionResult> TakeAll()
        {
            try
            {
                var result = await fridgeModelService.TakeFridgeModels();
                return View(result);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("TakeById/{id}")]
        public async Task<ActionResult> TakeById([FromRoute]Guid id)
        {
            try
            {
                var result = await fridgeModelService.TakeFridgeModelById(id);
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
            return View(new FridgeModel());
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(FridgeModel fridgeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await fridgeModelService.AddFridgeModel(fridgeModel);
                    return RedirectToAction(nameof(TakeAll));
                } 
                else
                    return View(fridgeModel);
                
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            } 
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            try
            {
                var fridgeModel = await fridgeModelService.TakeFridgeModelById(id);
                await fridgeModelService.DeleteFridgeModel(fridgeModel);
                return RedirectToAction(nameof(TakeAll));
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<ActionResult> Update([FromRoute]Guid id)
        {
            try
            {
                return View("Update",await fridgeModelService.TakeFridgeModelById(id));
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(FridgeModel fridgeModel)
        {try
                {
            if (ModelState.IsValid)
            {         
                    await fridgeModelService.UpdateFridgeModel(fridgeModel);
                    return RedirectToAction(nameof(TakeAll));
            }
            else
                return View(fridgeModel);
            }
            catch (HttpRequestException e)
                {
                    return CatchHttpRequestExeption(e);
                }
        }
    }
}
