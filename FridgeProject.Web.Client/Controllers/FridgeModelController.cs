using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FridgeProject.Abstract.Data;
using System.Net.Http;
using FridgeProject.Abstract;

namespace FridgeProject.Web.Client.Controllers
{
    [Route("/FridgeModel")]
    public class FridgeModelController : BaseController
    {
        private readonly IFridgeModelServices _fridgeModelService;

        public FridgeModelController(IFridgeModelServices fridgeModelService)
        {
          _fridgeModelService = fridgeModelService;
        }

        [HttpGet("TakeAll")]
        public async Task<ActionResult> TakeAll()
        {
            try
            {
                var result = await _fridgeModelService.TakeFridgeModels();
                return View(result);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> TakeById([FromRoute]Guid id)
        {
            try
            {
                var result = await _fridgeModelService.TakeFridgeModelById(id);
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
                    await _fridgeModelService.AddFridgeModel(fridgeModel);
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
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _fridgeModelService.DeleteFridgeModel(id);
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
                return View("Update",await _fridgeModelService.TakeFridgeModelById(id));
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(FridgeModel fridgeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {         
                        await _fridgeModelService.UpdateFridgeModel(fridgeModel);
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
