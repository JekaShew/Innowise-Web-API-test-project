using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Controllers
{
    [Route("/Fridge")]
    public class FridgeController : Controller
    {
        private readonly IFridge fridgeServices;
        private readonly IProduct productServices;
        private readonly IFridgeModel fridgeModelServices;

        public FridgeController(IFridge fridgeServices, IProduct productServices, IFridgeModel fridgeModelServices)
        {
            this.fridgeServices = fridgeServices;
            this.productServices = productServices;
            this.fridgeModelServices = fridgeModelServices;
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetFridges()
        {
            try
            {
                var result = await fridgeServices.GetFridges();
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
                return View();
            }
        }

        [HttpGet("GetFridgeById")]
        public async Task<ActionResult> GetFridgeById(Guid id)
         {
            try
            {
                var result = await fridgeServices.GetFridgeById(id);
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
        public async Task<ActionResult> AddFridge()
        {
            var FridgeWithFridgeModels = new FridgeWithFridgeModels
            {
                Fridge = new Fridge(),
                FridgeModels = await fridgeModelServices.GetFridgeModels(),
            };
            if (HttpContext.Response.StatusCode == 404)
                return View("NotFound");
            if (HttpContext.Response.StatusCode == 401)
                return View("Unauthorized");
            if (HttpContext.Response.StatusCode == 403)
                return View("AccessDenied");
            return View(FridgeWithFridgeModels); 
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddFridge(FridgeWithFridgeModels fridgeWithFridgeModels)
        {
            try
            {
                var fridge = new Fridge();
                fridge.Title = fridgeWithFridgeModels.Fridge.Title;
                fridge.OwnerName = fridgeWithFridgeModels.Fridge.OwnerName;
                fridge.FridgeModel = await fridgeModelServices.GetFridgeModelById(fridgeWithFridgeModels.FridgeModelId);
                fridge.FridgeProducts = new List<FridgeProduct>();
                if (ModelState.IsValid)
                {
                    await fridgeServices.AddFridge(fridge);
                    return RedirectToAction(nameof(GetFridges));
                }
                else
                    return View(fridge);
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

        [HttpGet("Delete")]
        public async Task<ActionResult> DeleteFridge(Guid id)
        {
            try
            {
                var product = await fridgeServices.GetFridgeById(id);
                if (product != null)
                {
                    await fridgeServices.DeleteFridge(product);
                    return RedirectToAction(nameof(GetFridges));
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
        public async Task<ActionResult> UpdateFridge(Guid id)
        {           
            return View(new FridgeWithFridgeModels
            {
                Fridge = await fridgeServices.GetFridgeById(id),
                FridgeModels = await fridgeModelServices.GetFridgeModels(),
                FridgeModelId = (await fridgeServices.GetFridgeById(id)).FridgeModel.Id
            });
        }
        [HttpPost("Update")]
        public async Task<ActionResult> UpdateFridge(FridgeWithFridgeModels fridgeWithFridgeModels)
        {
            
            var fridge = fridgeWithFridgeModels.Fridge;
            if (ModelState.IsValid)
            {
                try
                {
                    fridge.FridgeProducts = (await fridgeServices.GetFridgeById(fridge.Id)).FridgeProducts;
                    fridge.FridgeModel = await fridgeModelServices.GetFridgeModelById(fridgeWithFridgeModels.FridgeModelId);
                    await fridgeServices.UpdateFridge(fridge);
                    return RedirectToAction(nameof(GetFridges));
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
                fridgeWithFridgeModels.FridgeModels = await fridgeModelServices.GetFridgeModels();
                return View(fridgeWithFridgeModels);
        }

        [HttpGet("UpdateFridgesQuantity")]
        public async Task<ActionResult> UpdateFridgesQuantityGet()
        {
            return View("UpdateFridgesQuantity", new List<Fridge>());
        }

        [HttpPost("UpdateFridgesQuantity")]
        public async Task<ActionResult> UpdateFridgesQuantity()
        {
            try
            {
                var updatedFrigesWithoutQuantity = await fridgeServices.GetUpdatedFridgesWithoutQuantity();
                return View(updatedFrigesWithoutQuantity);
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

        [HttpGet("SelectProducts")]
        public async Task<IActionResult> SelectProducts(Guid fridgeId)
        {
            try
            {
                var products = await productServices.GetProducts();
                var fridgeWithProducts = new FridgeWithProducts
                {
                    Fridge = await fridgeServices.GetFridgeById(fridgeId),
                    Products = products
                };
                return View(fridgeWithProducts);
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

        [HttpGet("SelectedProduct")]
        public async Task<IActionResult> SelectedProduct(Guid productId, Guid fridgeId)
        {
            try
            {
                var product = await productServices.GetProductById(productId);

                var selectedFridgeProduct = new FridgeProduct
                {
                    Fridge = await fridgeServices.GetFridgeById(fridgeId),
                    Product = product,
                    Quantity = product.DefaultQuantity.Value
                };
                return View(selectedFridgeProduct);
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

        [HttpPost("SelectedProduct")]
        public async Task<IActionResult> SelectedProduct(SelectedProduct selectedProductToAdd)
        {
            
            var fridge = await fridgeServices.GetFridgeById(selectedProductToAdd.FridgeId);
            var selectedProduct = await productServices.GetProductById(selectedProductToAdd.ProductId);
            var existingFridgeProduct = fridge.FridgeProducts.FirstOrDefault(fp => fp.Product.Equals(selectedProduct));

            var selectedFridgeProduct = new FridgeProduct
            {
                Fridge = await fridgeServices.GetFridgeById(selectedProductToAdd.FridgeId),
                Product = selectedProduct,
                Quantity = selectedProductToAdd.Quantity
            };
            if (ModelState.IsValid)
            {
                try
                {
                    if (existingFridgeProduct != null)
                    {
                        existingFridgeProduct.Quantity += selectedProductToAdd.Quantity;

                    }
                    else
                        fridge.FridgeProducts.Add(new FridgeProduct
                        {
                            Id = Guid.NewGuid(),
                            Fridge = fridge,
                            Product = selectedProduct,
                            Quantity = selectedProductToAdd.Quantity
                        });

                    await fridgeServices.UpdateFridge(fridge);
                    return Redirect($"/Fridge/SelectProducts?FridgeId={selectedProductToAdd.FridgeId}");
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
            else return View(selectedFridgeProduct);
        }

        [HttpGet("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(Guid productId, Guid fridgeId)
        {
            try
            {
                var product = await productServices.GetProductById(productId);
                var fridge = await fridgeServices.GetFridgeById(fridgeId);
                fridge.FridgeProducts.Remove(fridge.FridgeProducts.FirstOrDefault(fp => fp.Product.Equals(product)));
                await fridgeServices.UpdateFridge(fridge);
                return Redirect($"/Fridge/GetFridgeById?Id={fridgeId}");
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

    }
}
