using AutoMapper;
using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using FridgeProject.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FridgeProject.Tests
{
    public class FridgeServicesTests
    {
        private (AppDBContext, FridgeServices, FridgeModelServices, ProductServices) Init()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var dbContext = new AppDBContext(options);
            var config = new MapperConfiguration(cfg => cfg.AddProfile<Services.AutoMapper>());
            var mapper = config.CreateMapper();
            var service = new FridgeServices(dbContext, mapper);
            var fridgeModelService = new FridgeModelServices(dbContext,mapper);
            var productService = new ProductServices(dbContext, mapper);
            return (dbContext, service, fridgeModelService, productService);
        }

        [Fact]
        public async void AddFridge_ValidModel_DataWasSaved()
        {
            var (dbContext, service, fridgeModelService, productService) = Init();
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {   
                Id = Guid.NewGuid(),
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-770" && x.Year == 2005));
            
            var fridgeModel = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.First().Id);          
            await service.AddFridge(new Fridge()
            {
                Id = Guid.NewGuid(),
                Title ="Fridge with beer", 
                OwnerName ="Vasiliy Ivanovich Fedotov", 
                FridgeModel = fridgeModel, 
                FridgeProducts = new List<FridgeProduct>() 
            });

            Assert.True(await dbContext.Fridges.AnyAsync (x => x.Title == "Fridge with beer" && x.OwnerName == "Vasiliy Ivanovich Fedotov"));
        }
        
        [Fact]
        public async void TakeByIdFridge_ValidModel_DataExist()
        {
            var (dbContext, service, fridgeModelService, productService) = Init();
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-770" && x.Year == 2005));
           
            var fridgeModel = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.First().Id);
            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with beer",
                OwnerName = "Vasiliy Ivanovich Fedotov",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with beer" && x.OwnerName == "Vasiliy Ivanovich Fedotov"));

            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with vodka",
                OwnerName = "Ivan Ivanovich Ivanov",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with vodka" && x.OwnerName == "Ivan Ivanovich Ivanov"));

            var fridge = await service.TakeFridgeById(dbContext.Fridges.FirstOrDefault(f => f.Title == "Fridge with beer").Id);

            Assert.True(fridge.Title == "Fridge with beer"
                && fridge.OwnerName == "Vasiliy Ivanovich Fedotov"
                && fridge.FridgeModel.Title == fridgeModel.Title);
        }

        [Fact]
        public async void DeleteFridge_ValidModel_DataWasDeleted()
        {
            var (dbContext, service, fridgeModelService, productService) = Init();
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-770" && x.Year == 2005));
            
            var fridgeModel = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.First().Id);
            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with beer",
                OwnerName = "Vasiliy Ivanovich Fedotov",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with beer" && x.OwnerName == "Vasiliy Ivanovich Fedotov"));

            await service.DeleteFridge(dbContext.Fridges.First().Id);

            Assert.False(await dbContext.Fridges.AnyAsync(f => f.Title == "Fridge with beer" && f.OwnerName == "Vasiliy Ivanovich Fedotov" && f.FridgeModelId == fridgeModel.Id));
        }  
              
        [Fact]
         public async void UpdateFridge_ValidModel_DataWasUpdated()
        {
            var (dbContext, service, fridgeModelService, productService) = Init();
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-770" && x.Year == 2005));
            
            var fridgeModel1 = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "LG-770").Id);
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "ZELMA-70",
                Year = 2011
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "ZELMA-70" && x.Year == 2011));

            await productService.AddProduct(new Product { Title = "Cucumber", DefaultQuantity = 3 });
            await productService.AddProduct(new Product { Title = "Tomato", DefaultQuantity = 2 });  
            var products = await productService.TakeProducts();
            var fridgeModel2 = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "ZELMA-70").Id);
            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with beer",
                OwnerName = "Vasiliy Ivanovich Fedotov",
                FridgeModel = fridgeModel1,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with beer" && x.OwnerName == "Vasiliy Ivanovich Fedotov"));

            await service.UpdateFridge( new Fridge
            {
                Id = dbContext.Fridges.FirstOrDefault().Id,
                Title = "Bad fridge",
                OwnerName = "Petrikov Genadiy viktorovich",
                FridgeModel = fridgeModel2,
                FridgeProducts = (await service.TakeFridgeById(dbContext.Fridges.FirstOrDefault().Id)).FridgeProducts
            });

            Assert.True(
                await dbContext.Fridges.AnyAsync(f => 
                    f.Title == "Bad fridge"
                    && f.OwnerName == "Petrikov Genadiy viktorovich"
                    && f.FridgeModelId == fridgeModel2.Id)
                && !await dbContext.Fridges.AnyAsync(f => 
                    f.Title == "Fridge with beer"
                    && f.OwnerName == "Vasiliy Ivanovich Fedotov"
                    && f.FridgeModelId == fridgeModel1.Id)
                );
        }

        [Fact]
        public async void TakeAllFridges_ValdiModel_DataExist()
        {
            var (dbContext, service, fridgeModelService, productService) = Init();
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-770" && x.Year == 2005));
            
            var fridgeModel1 = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "LG-770").Id);
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "ZELMA-70",
                Year = 2011
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "ZELMA-70" && x.Year == 2011));
           
            var fridgeModel2 = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "ZELMA-70").Id);
            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with beer",
                OwnerName = "Vasiliy Ivanovich Fedotov",
                FridgeModel = fridgeModel1,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with beer" && x.OwnerName == "Vasiliy Ivanovich Fedotov"));

            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with vodka",
                OwnerName = "Petr Ivanovich Kadjitov",
                FridgeModel = fridgeModel2,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with vodka" && x.OwnerName == "Petr Ivanovich Kadjitov"));

            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with energy drink",
                OwnerName = "Ivan Ivanovich Slizerin",
                FridgeModel = fridgeModel1,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with energy drink" && x.OwnerName == "Ivan Ivanovich Slizerin"));

            var fridges = await service.TakeFridges();

            Assert.True(
                fridges.Count == 3
                && fridges.Any(f1 => 
                    f1.Title == "Fridge with beer"
                    && f1.OwnerName == "Vasiliy Ivanovich Fedotov"
                    && f1.FridgeModel.Title == fridgeModel1.Title)
                && fridges.Any(f2 =>
                    f2.Title == "Fridge with vodka"
                    && f2.OwnerName == "Petr Ivanovich Kadjitov"
                    && f2.FridgeModel.Title == fridgeModel2.Title)
                && fridges.Any( f3 =>
                    f3.Title == "Fridge with energy drink"
                    && f3.OwnerName == "Ivan Ivanovich Slizerin"
                    && f3.FridgeModel.Title == fridgeModel1.Title)
                );
        }

        [Fact]
        public async void UpdateFridgeProductsQuantity_ValidModel_QuantityWasUpdated()
        {
            var (dbContext, service, fridgeModelService, productService) = Init();
            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-770" && x.Year == 2005));
            
            var fridgeModel = await fridgeModelService.TakeFridgeModelById(dbContext.FridgeModels.First().Id);
            await productService.AddProduct(new Product { Title = "Cucumber", DefaultQuantity = 3 });
            await productService.AddProduct(new Product { Title = "Tomato", DefaultQuantity = 2 });
            await productService.AddProduct(new Product { Title = "Cabagge", DefaultQuantity = 1 });
            var products = await dbContext.Products.ToListAsync();
            await service.AddFridge(new Fridge()
            {
                Title = "Fridge with beer",
                OwnerName = "Vasiliy Ivanovich Fedotov",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>() 
            });

            Assert.True(await dbContext.Fridges.AnyAsync(x => x.Title == "Fridge with beer" && x.OwnerName == "Vasiliy Ivanovich Fedotov"));
           
            var fridge = await dbContext.Fridges.FirstOrDefaultAsync(x => x.Title == "Fridge with beer" && x.OwnerName == "Vasiliy Ivanovich Fedotov");
            dbContext.FridgeProducts.AddRange(
                new List<Data.Models.FridgeProduct>
                {
                    new Data.Models.FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridge.Id,
                        Quantity = 4
                    },
                    new Data.Models.FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[1].Id,
                        FridgeId = fridge.Id,
                        Quantity = 0
                    },
                    new Data.Models.FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[2].Id,
                        FridgeId = fridge.Id,
                        Quantity = 4
                    }
                });
           await dbContext.SaveChangesAsync();
           var updatedFridges = await service.TakeUpdatedFridgesWithoutQuantity();

            Assert.True(
                updatedFridges.Count == 1
                && updatedFridges.Any(uf =>
                    uf.Title == "Fridge with beer"
                    && uf.OwnerName == "Vasiliy Ivanovich Fedotov"
                    && uf.FridgeModel.Title == "LG-770"
                    && !uf.FridgeProducts.Any(fp => fp.Quantity == 0)
                ));
        }
    }
}
