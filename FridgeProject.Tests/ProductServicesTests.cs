using AutoMapper;
using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using FridgeProject.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace FridgeProject.Tests
{
    public class ProductServicesTests
    {
        private (AppDBContext, ProductServices) Init()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<Services.AutoMapper>());
            var mapper = config.CreateMapper();
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var dbContext = new AppDBContext(options);
            var service = new ProductServices(dbContext, mapper);
            return (dbContext, service);
        }
        [Fact]
        public async void AddProduct_ValidModel_DataWasSaved()
        {
            var (dbContext, service) = Init();
            await service.AddProduct(new Product()
            {
                Title = "Tomato",
                DefaultQuantity = 3
            });

            Assert.True(await dbContext.Products.AnyAsync(x => x.Title == "Tomato"));
        }

        [Fact]
        public async void TakeByIdProduct__ValidModel_DataExist()
        {
            var (dbContext, service) = Init();
            await service.AddProduct(new Product()
            {
                Title = "Tomato",
                DefaultQuantity = 3
            });

            Assert.True(await dbContext.Products.AnyAsync(x => x.Title == "Tomato"));

            var product = await service.TakeProductById(dbContext.Products.First().Id);

            Assert.True(product.Title == "Tomato" && product.DefaultQuantity == 3);
        }

        [Fact]
        public async void DeleteProduct_ValidModel_DataWasDeleted()
        {
            var (dbContext, service) = Init();
            await service.AddProduct(new Product()
            {
                Title = "Sauce",
                DefaultQuantity = 3
            });

            Assert.True(await dbContext.Products.AnyAsync(x => x.Title == "Sauce"));

            var product = await dbContext.Products.FirstOrDefaultAsync(fm => fm.Title == "Sauce");
            await service.DeleteProduct(product.Id);

            Assert.False(await dbContext.Products.AnyAsync(x => x.Title == "Sauce"));
        }


        [Fact]
        public async void UpdateProduct_ValidModel_DataWasUpdated()
        {
            var (dbContext, service) = Init();
            await service.AddProduct(new Product()
            {
                Title = "Chicken",
                DefaultQuantity = 2
            });

            Assert.True(await dbContext.Products.AnyAsync(x => x.Title == "Chicken"));

            var product = await dbContext.Products.FirstOrDefaultAsync(fm => fm.Title == "Chicken");
            await service.UpdateProduct(new Product { Id = dbContext.Products.First(fm => fm.Title == "Chicken").Id, Title = "Cucumber", DefaultQuantity = 3 });

            Assert.True(await dbContext.Products.AnyAsync(x => x.Title == "Cucumber" && x.DefaultQuantity == 3) && !dbContext.FridgeModels.Any(x => x.Title == "Chicken" && x.Year == 2));
        }

        [Fact]
        public async void TakeProducts_ValidModels_DataExist()
        {
            var (dbContext, service) = Init();
            await service.AddProduct(new Product()
            {
                Title = "Tomato",
                DefaultQuantity = 2
            });

            Assert.True(await dbContext.Products.AnyAsync (x => x.Title == "Tomato"));

            await service.AddProduct(new Product()
            {
                Title = "Cucumber",
                DefaultQuantity = 3
            });

            Assert.True(await dbContext.Products.AnyAsync(x => x.Title == "Cucumber"));

            await service.AddProduct(new Product()
            {
                Title = "Pumpkin",
                DefaultQuantity = 5
            });

            Assert.True(await dbContext.Products.AnyAsync(x => x.Title == "Pumpkin"));

            var products = await service.TakeProducts();

            Assert.True(
                products.Count == 3
                && products.Any(p => p.Title == "Tomato" && p.DefaultQuantity == 2)
                && products.Any(p => p.Title == "Cucumber" && p.DefaultQuantity == 3)
                && products.Any(p => p.Title == "Pumpkin" && p.DefaultQuantity == 5)
                );
        }
    }
}
