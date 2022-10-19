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
    public class FridgeController : BaseController
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
                return CatchHttpRequestExeption(e);
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
                return CatchHttpRequestExeption(e);
            }
        }
       
        [HttpGet("Add")]
        public async Task<ActionResult> AddFridge()
        {
            var fridgeWithFridgeModels = new FridgeWithFridgeModels
            {
                Fridge = new Fridge(),
                FridgeModels = await fridgeModelServices.GetFridgeModels(),
            };
            return View(fridgeWithFridgeModels);
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
                return CatchHttpRequestExeption(e);
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
                return CatchHttpRequestExeption(e);
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
           try
            { 
                var fridge = fridgeWithFridgeModels.Fridge;
                if (ModelState.IsValid)
                {
                
                        fridge.FridgeProducts = (await fridgeServices.GetFridgeById(fridge.Id)).FridgeProducts;
                        fridge.FridgeModel = await fridgeModelServices.GetFridgeModelById(fridgeWithFridgeModels.FridgeModelId);
                        await fridgeServices.UpdateFridge(fridge);
                        return RedirectToAction(nameof(GetFridges));
                
                }
                else
                    fridgeWithFridgeModels.FridgeModels = await fridgeModelServices.GetFridgeModels();
                    return View(fridgeWithFridgeModels);}
           catch (HttpRequestException e)
           {
               return CatchHttpRequestExeption(e);
           }
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
                return CatchHttpRequestExeption(e);
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
                return CatchHttpRequestExeption(e);
            }
        }
        [HttpGet("SelectExistedProducts")]
        public async Task<IActionResult> SelectExistedProducts(Guid fridgeId)
        {
            try
            {
                var existedProducts = (await fridgeServices.GetFridgeById(fridgeId)).FridgeProducts.ToList();
                var fridgeWithExistedProducts = new FridgeWithExistedProducts
                {
                    Fridge = await fridgeServices.GetFridgeById(fridgeId),
                    ExistedProducts = existedProducts
                };
                return View(fridgeWithExistedProducts);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
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
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("SelectedExistedProduct")]
        public async Task<IActionResult> SelectedExistedProduct(Guid fridgeProductId,Guid fridgeId)
        {
            try
            {
                var fridgeProduct = (await fridgeServices.GetFridgeById(fridgeId)).FridgeProducts.FirstOrDefault(fp => fp.Id == fridgeProductId);

                return View(fridgeProduct);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpPost("SelectedProduct")]
        public async Task<IActionResult> SelectedProduct(SelectedProduct selectedProductToPut)
        {
            try
            {
                var fridge = await fridgeServices.GetFridgeById(selectedProductToPut.FridgeId);
                var selectedProduct = await productServices.GetProductById(selectedProductToPut.ProductId);
                var existingFridgeProduct = fridge.FridgeProducts.FirstOrDefault(fp => fp.Product.Equals(selectedProduct));

                var selectedFridgeProduct = new FridgeProduct
                {
                    Fridge = await fridgeServices.GetFridgeById(selectedProductToPut.FridgeId),
                    Product = selectedProduct,
                    Quantity = selectedProductToPut.Quantity
                };
                if (ModelState.IsValid)
                {
                        if (existingFridgeProduct != null)
                        {
                            existingFridgeProduct.Quantity += selectedProductToPut.Quantity;

                        }
                        else
                            fridge.FridgeProducts.Add(new FridgeProduct
                            {
                                Fridge = fridge,
                                Product = selectedProduct,
                                Quantity = selectedProductToPut.Quantity
                            });

                        await fridgeServices.UpdateFridge(fridge);
                        return Redirect($"/Fridge/SelectProducts?FridgeId={selectedProductToPut.FridgeId}");
                }
                else return View(selectedFridgeProduct);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpPost("SelectedExistedProduct")]
        public async Task<IActionResult> SelectedExistedProduct(SelectedFridgeProduct selectedFridgeProductToTake)
        {
            try
            {
                var fridge = await fridgeServices.GetFridgeById(selectedFridgeProductToTake.FridgeId);
                var selecteFridgeProduct = fridge
                    .FridgeProducts.FirstOrDefault(fp => fp.Id == selectedFridgeProductToTake.FridgeProductId);

                if (ModelState.IsValid)
                {
                    if (selectedFridgeProductToTake.Quantity <= selecteFridgeProduct.Quantity)
                    {
                        ViewBag.QuantityError = "";
                        fridge.FridgeProducts.FirstOrDefault(fp =>
                            fp.Id == selecteFridgeProduct.Id)
                            .Quantity = selecteFridgeProduct.Quantity - selectedFridgeProductToTake.Quantity;

                        await fridgeServices.UpdateFridge(fridge);
                        return Redirect($"/Fridge/GetFridgeById?Id={selectedFridgeProductToTake.FridgeId}");
                    }
                    else
                    {
                        ViewBag.QuantityError = "You can't Take more than exists!";
                        return View(selecteFridgeProduct);
                    }
                }
                else return View(selecteFridgeProduct);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
            
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
                return CatchHttpRequestExeption(e);
            }
        }
    }
}
