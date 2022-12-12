using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Web.Client.Models;
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
        private readonly IFridgeServices _fridgeServices;
        private readonly IProductServices _productServices;
        private readonly IFridgeModelServices _fridgeModelServices;

        public FridgeController(IFridgeServices fridgeServices, IProductServices productServices, IFridgeModelServices fridgeModelServices)
        {
            _fridgeServices = fridgeServices;
            _productServices = productServices;
            _fridgeModelServices = fridgeModelServices;
        }

        [HttpGet("TakeAll")]
        public async Task<ActionResult> TakeAll()
        {
            try
            {
                var result = await _fridgeServices.TakeFridges();
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
                var result = await _fridgeServices.TakeFridgeById(id);
                return View(result);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }
       
        [HttpGet("Add")]
        public async Task<ActionResult> Add()
        {
            var fridgeWithFridgeModels = new FridgeWithFridgeModels
            {
                Fridge = new Fridge(),
                FridgeModels = await _fridgeModelServices.TakeFridgeModels(),
            };
            return View(fridgeWithFridgeModels);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm]FridgeWithFridgeModels fridgeWithFridgeModels)
        {
            try
            {
                var fridge = new Fridge();
                fridge.Title = fridgeWithFridgeModels.Fridge.Title;
                fridge.OwnerName = fridgeWithFridgeModels.Fridge.OwnerName;
                fridge.FridgeModel = await _fridgeModelServices.TakeFridgeModelById(fridgeWithFridgeModels.FridgeModelId);
                fridge.FridgeProducts = new List<FridgeProduct>();
                if (ModelState.IsValid)
                {
                    await _fridgeServices.AddFridge(fridge);
                    return RedirectToAction(nameof(TakeAll));
                }
                else
                    return View(fridge);
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
                await _fridgeServices.DeleteFridge(id);
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
            var fridge = await _fridgeServices.TakeFridgeById(id);
            return base.View(new FridgeWithFridgeModels
            {
                Fridge = fridge,
                FridgeModels = await _fridgeModelServices.TakeFridgeModels(),
                FridgeModelId = fridge.FridgeModel.Id
            });
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update([FromForm]FridgeWithFridgeModels fridgeWithFridgeModels)
        {
           try
            { 
                var fridge = fridgeWithFridgeModels.Fridge;
                if (ModelState.IsValid)
                {   
                        fridge.FridgeProducts = (await _fridgeServices.TakeFridgeById(fridge.Id)).FridgeProducts;
                        fridge.FridgeModel = await _fridgeModelServices.TakeFridgeModelById(fridgeWithFridgeModels.FridgeModelId);
                        await _fridgeServices.UpdateFridge(fridge);
                        return RedirectToAction(nameof(TakeAll));
                }
                else
                    fridgeWithFridgeModels.FridgeModels = await _fridgeModelServices.TakeFridgeModels();
                    return View(fridgeWithFridgeModels);}
           catch (HttpRequestException e)
           {
               return CatchHttpRequestExeption(e);
           }
        }

        [HttpGet("UpdateFridgesQuantity")]
        public ActionResult ShowUpdateFridgesQuantity()
        {
            return View("UpdateFridgesQuantity", new List<Fridge>());
        }

        [HttpPost("UpdateFridgesQuantity")]
        public async Task<ActionResult> UpdateFridgesQuantity()
        {
            try
            {
                var updatedFrigesWithoutQuantity = await _fridgeServices.TakeUpdatedFridgesWithoutQuantity();
                return View("UpdateFridgesQuantity", updatedFrigesWithoutQuantity);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }     
        }

        [HttpGet("SelectProducts/{fridgeId}")]
        public async Task<IActionResult> SelectProducts([FromRoute]Guid fridgeId)
        {
            try
            {
                var products = await _productServices.TakeProducts();
                var fridgeWithProducts = new FridgeWithProducts
                {
                    Fridge = await _fridgeServices.TakeFridgeById(fridgeId),
                    Products = products
                };
                return View(fridgeWithProducts);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("SelectedProduct/{productId}/{fridgeId}")]
        public async Task<IActionResult> ShowSelectedProduct([FromRoute]Guid productId, Guid fridgeId)
        {
            try
            {
                var product = await _productServices.TakeProductById(productId);
                var selectedFridgeProduct = new FridgeProduct
                {
                    Fridge = await _fridgeServices.TakeFridgeById(fridgeId),
                    Product = product,
                    Quantity = product.DefaultQuantity.Value
                };
                return View("SelectedProduct", selectedFridgeProduct);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpPost("SelectedProduct")]
        public async Task<IActionResult> AddSelectedProduct([FromForm]SelectedProduct selectedProductToPut)
        {
            try
            {
                var fridge = await _fridgeServices.TakeFridgeById(selectedProductToPut.FridgeId);
                var selectedProduct = await _productServices.TakeProductById(selectedProductToPut.ProductId);
                var existingFridgeProduct = fridge.FridgeProducts.FirstOrDefault(fp => fp.Product.Equals(selectedProduct));
                var selectedFridgeProduct = new FridgeProduct
                {
                    Fridge = await _fridgeServices.TakeFridgeById(selectedProductToPut.FridgeId),
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

                    await _fridgeServices.UpdateFridge(fridge);
                    return Redirect($"/Fridge/SelectProducts/{selectedProductToPut.FridgeId}");
                }
                else return View("SelectedProduct",selectedFridgeProduct);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpGet("SelectExistedProducts/{fridgeId}")]
        public async Task<IActionResult> SelectExistedProducts([FromRoute]Guid fridgeId)
        {
            try
            {
                var existedProducts = (await _fridgeServices.TakeFridgeById(fridgeId)).FridgeProducts.ToList();
                var fridgeWithExistedProducts = new FridgeWithExistedProducts
                {
                    Fridge = await _fridgeServices.TakeFridgeById(fridgeId),
                    ExistedProducts = existedProducts
                };
                return View("SelectExistedProducts",fridgeWithExistedProducts);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        

        [HttpGet("SelectedExistedProduct/{fridgeProductId}/{fridgeId}")]
        public async Task<IActionResult> ShowSelectedExistedProduct([FromRoute]Guid fridgeProductId,Guid fridgeId)
        {
            try
            {
                var fridgeProduct = (await _fridgeServices.TakeFridgeById(fridgeId)).FridgeProducts.FirstOrDefault(fp => fp.Id == fridgeProductId);
                fridgeProduct.Fridge = await _fridgeServices.TakeFridgeById(fridgeId);
                return View("SelectedExistedProduct",fridgeProduct);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }

        [HttpPost("SelectedExistedProduct")]
        public async Task<IActionResult> TakeSelectedExistedProduct([FromForm]SelectedFridgeProduct selectedFridgeProductToTake)
        {
            try
            {
                var fridge = await _fridgeServices.TakeFridgeById(selectedFridgeProductToTake.FridgeId);
                var selectedFridgeProduct = fridge
                    .FridgeProducts.FirstOrDefault(fp => fp.Product.Id == selectedFridgeProductToTake.FridgeProductId);
                if (ModelState.IsValid)
                {
                    if (selectedFridgeProductToTake.Quantity <= selectedFridgeProduct.Quantity)
                    {
                        ViewBag.QuantityError = "";
                        fridge.FridgeProducts.FirstOrDefault(fp =>
                            fp.Id == selectedFridgeProduct.Id)
                            .Quantity = selectedFridgeProduct.Quantity - selectedFridgeProductToTake.Quantity;
                        await _fridgeServices.UpdateFridge(fridge);
                        return Redirect($"/Fridge/{selectedFridgeProductToTake.FridgeId}");
                    }
                    else
                    {
                        ViewBag.QuantityError = "You can't Take more than exists!";
                        return View("SelectedExistedProduct",selectedFridgeProduct);
                    }
                }
                else return View("SelectedExistedProduct", selectedFridgeProduct);
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }   
        }

        [HttpGet("DeleteProduct/{productId}/{fridgeId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute]Guid productId, Guid fridgeId)
        {
            try
            {
                var product = await _productServices.TakeProductById(productId);
                var fridge = await _fridgeServices.TakeFridgeById(fridgeId);
                fridge.FridgeProducts.Remove(fridge.FridgeProducts.FirstOrDefault(fp => fp.Product.Equals(product)));
                await _fridgeServices.UpdateFridge(fridge);
                return Redirect($"/Fridge/{fridgeId}");
            }
            catch (HttpRequestException e)
            {
                return CatchHttpRequestExeption(e);
            }
        }
    }
}
