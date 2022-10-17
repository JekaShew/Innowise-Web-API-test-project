﻿using Microsoft.AspNetCore.Mvc;
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
    public class FridgeModelController : Controller
    {
        private readonly IFridgeModel fridgeModelService;

        public FridgeModelController(IFridgeModel fridgeModelService)
        {
            this.fridgeModelService = fridgeModelService;
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetFridgeModels()
        {
            try
            {
                var result = await fridgeModelService.GetFridgeModels();
                return View(result);
            }
            catch (HttpRequestException e)
            {
                if ((int)e.StatusCode == 404)
                    return View("~/Views/Errors/NotFound.cshtml");
                if ((int)e.StatusCode == 401)
                    return View("~/Views/Errors/Unauthorized.cshtml");
                if ((int)e.StatusCode == 403)
                    return View("~/Views/Errors/AccessDenied.cshtml");
                //if (HttpContext.Response.StatusCode == 200)
                return View();

            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFridgeModelById(Guid id)
        {
            try
            {
                var result = await fridgeModelService.GetFridgeModelById(id);
                if (result != null)
                {
                    return View(result);
                }
                else
                    return View();
            }
            catch (HttpRequestException e)
            {
                if ((int)e.StatusCode == 404)
                    return View("~/Views/Errors/NotFound.cshtml");
                if ((int)e.StatusCode == 401)
                    return View("~/Views/Errors/Unauthorized.cshtml");
                if ((int)e.StatusCode == 403)
                    return View("~/Views/Errors/AccessDenied.cshtml");
                return View();
            }


        }

        [HttpGet("Add")]
        public ActionResult AddFridgeModel()
        {
            return View(new FridgeModel());
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddFridgeModel(FridgeModel fridgeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await fridgeModelService.AddFridgeModel(fridgeModel);
                    return RedirectToAction(nameof(GetFridgeModels));
                }
                catch (HttpRequestException e)
                {
                    if ((int)e.StatusCode == 404)
                        return View("~/Views/Errors/NotFound.cshtml");
                    if ((int)e.StatusCode == 401)
                        return View("~/Views/Errors/Unauthorized.cshtml");
                    if ((int)e.StatusCode == 403)
                        return View("~/Views/Errors/AccessDenied.cshtml");
                    return View();
                }
            }
            else
                return View(fridgeModel);
        }


        [HttpGet("Delete")]
        public async Task<ActionResult> DeleteFridgeModel(Guid id)
        {
            try
            {
                var fridgeModel = await fridgeModelService.GetFridgeModelById(id);
                if (fridgeModel != null)
                {
                    await fridgeModelService.DeleteFridgeModel(fridgeModel);
                    return RedirectToAction(nameof(GetFridgeModels));
                }
                else
                    return View();
            }
            catch (HttpRequestException e)
            {
                if ((int)e.StatusCode == 404)
                    return View("~/Views/Errors/NotFound.cshtml");
                if ((int)e.StatusCode == 401)
                    return View("~/Views/Errors/Unauthorized.cshtml");
                if ((int)e.StatusCode == 403)
                    return View("~/Views/Errors/AccessDenied.cshtml");
                return View();
            }
        }

        [HttpGet("Update")]
        public async Task<ActionResult> UpdateFridgeModel(Guid id)
        {
            try
            {
                return View(await fridgeModelService.GetFridgeModelById(id));
            }
            catch (HttpRequestException e)
            {
                if ((int)e.StatusCode == 404)
                    return View("~/Views/Errors/NotFound.cshtml");
                if ((int)e.StatusCode == 401)
                    return View("~/Views/Errors/Unauthorized.cshtml");
                if ((int)e.StatusCode == 403)
                    return View("~/Views/Errors/AccessDenied.cshtml");
                return View();
            }
        }
        [HttpPost("Update")]
        public async Task<ActionResult> UpdateFridgeModel(FridgeModel fridgeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    await fridgeModelService.UpdateFridgeModel(fridgeModel);
                    return RedirectToAction(nameof(GetFridgeModels));
                }
                catch (HttpRequestException e)
                {
                    if ((int)e.StatusCode == 404)
                        return View("~/Views/Errors/NotFound.cshtml");
                    if ((int)e.StatusCode == 401)
                        return View("~/Views/Errors/Unauthorized.cshtml");
                    if ((int)e.StatusCode == 403)
                        return View("~/Views/Errors/AccessDenied.cshtml");
                    //if (HttpContext.Response.StatusCode == 200)
                    return View();
                }
            }
            else
                return View(fridgeModel);
        }
    }
}
