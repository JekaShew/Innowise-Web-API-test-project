using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using FridgeProject.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FridgeProject.Tests
{
    public class ProductServicesTests
    {
        [Fact]
        public async void AddProductTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Add").Options;
            using var dbContext = new AppDBContext(options);
            var service = new ProductServices(dbContext);

            await service.AddProduct(new Product()
            {
                Title = "Томат",
                DefaultQuantity = 3
            });

            Assert.True(dbContext.Products.Any(x => x.Title == "Томат"));
        }

        [Fact]
        public async void GetByIdProductTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("GetById").Options;
            using var dbContext = new AppDBContext(options);
            var service = new ProductServices(dbContext);
            await service.AddProduct(new Abstract.Data.Product()
            {
                Title = "Томат",
                DefaultQuantity = 3
            });

            Assert.True(dbContext.Products.Any(x => x.Title == "Томат"));

            var product = await service.GetProductById(dbContext.Products.First().Id);

            Assert.True(product.Title == "Томат" && product.DefaultQuantity == 3);
        }

        [Fact]
        public async void DeleteProductTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Delete").Options;
            using var dbContext = new AppDBContext(options);
            var service = new ProductServices(dbContext);
            await service.AddProduct(new Abstract.Data.Product()
            {
                Title = "Кетчуп",
                DefaultQuantity = 3
            });

            Assert.True(dbContext.Products.Any(x => x.Title == "Кетчуп"));

            var product = await dbContext.Products.FirstOrDefaultAsync(fm => fm.Title == "Кетчуп");

            await service.DeleteProduct(await service.GetProductById(product.Id));

            Assert.False(dbContext.Products.Any(x => x.Title == "Кетчуп"));
        }


        [Fact]
        public async void UpdateProductTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Update").Options;
            using var dbContext = new AppDBContext(options);
            var service = new ProductServices(dbContext);
            await service.AddProduct(new Product()
            {
                Title = "Курица",
                DefaultQuantity = 2
            });

            Assert.True(dbContext.Products.Any(x => x.Title == "Курица"));

            var product = await dbContext.Products.FirstOrDefaultAsync(fm => fm.Title == "Курица");

            await service.UpdateProduct(new Product { Id = dbContext.Products.First(fm => fm.Title == "Курица").Id, Title = "Огурец", DefaultQuantity = 3 });

            Assert.True(dbContext.Products.Any(x => x.Title == "Огурец" && x.DefaultQuantity == 3) && !dbContext.FridgeModels.Any(x => x.Title == "Курица" && x.Year == 2));
        }

        [Fact]
        public async void GetFridgeModelsTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("GetAll").Options;
            using var dbContext = new AppDBContext(options);
            var service = new ProductServices(dbContext);

            await service.AddProduct(new Product()
            {
                Title = "Томат",
                DefaultQuantity = 2
            });

            Assert.True(dbContext.Products.Any(x => x.Title == "Томат"));

            await service.AddProduct(new Product()
            {
                Title = "Огурец",
                DefaultQuantity = 3
            });

            Assert.True(dbContext.Products.Any(x => x.Title == "Огурец"));

            await service.AddProduct(new Product()
            {
                Title = "Тыква",
                DefaultQuantity = 5
            });

            Assert.True(dbContext.Products.Any(x => x.Title == "Тыква"));

            var products = await service.GetProducts();

            Assert.True(
                products.Count == 3
                && products.Any(p => p.Title == "Томат" && p.DefaultQuantity == 2)
                && products.Any(p => p.Title == "Огурец" && p.DefaultQuantity == 3)
                && products.Any(p => p.Title == "Тыква" && p.DefaultQuantity == 5)
                );
        }
    }
}
