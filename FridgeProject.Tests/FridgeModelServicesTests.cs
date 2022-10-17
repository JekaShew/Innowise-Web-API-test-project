using FridgeProject.Abstract;
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
    public class FridgeModelServicesTests
    {
        [Fact]
        public async void AddFridgeModelTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Add").Options;
            using var dbContext = new AppDBContext(options);
            var service = new FridgeModelServices(dbContext);

            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-770"));
        }

        [Fact]
        public async void GetByIdFridgeModelTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("GetById").Options;
            using var dbContext = new AppDBContext(options);
            var service = new FridgeModelServices(dbContext);
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-170",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-170"));

            var fridgeModel = await service.GetFridgeModelById(dbContext.FridgeModels.First().Id);

            Assert.True(fridgeModel.Title == "LG-170" && fridgeModel.Year == 2005);
        }

        [Fact]
        public async void DeleteFridgeModelTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Delete").Options;
            using var dbContext = new AppDBContext(options);
            var service = new FridgeModelServices(dbContext);
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-970",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-970"));

            var fridgeModel = await dbContext.FridgeModels.FirstOrDefaultAsync(fm => fm.Title == "LG-970");

            await service.DeleteFridgeModel(await service.GetFridgeModelById(fridgeModel.Id));

            Assert.False(dbContext.FridgeModels.Any(x => x.Title == "LG-970"));
        }


        [Fact]
        public async void UpdateFridgeModelTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Update").Options;
            using var dbContext = new AppDBContext(options);
            var service = new FridgeModelServices(dbContext);
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-970",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-970"));

            var fridgeModel = await dbContext.FridgeModels.FirstOrDefaultAsync(fm => fm.Title == "LG-970");

            await service.UpdateFridgeModel(new FridgeModel {Id = dbContext.FridgeModels.First(fm=>fm.Title == "LG-970").Id, Title = "LG-2370", Year = 2010});

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-2370" && x.Year == 2010) && !dbContext.FridgeModels.Any(x => x.Title == "LG-970" && x.Year == 2005));
        }

        [Fact]
        public async void GetFridgeModelsTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("GetAll").Options;
            using var dbContext = new AppDBContext(options);
            var service = new FridgeModelServices(dbContext);

            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "Mi-270",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "Mi-270"));

            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-220",
                Year = 2006
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-220"));

            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "ZELMA-070",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "ZELMA-070"));

            var fridgeModels = await service.GetFridgeModels();

            Assert.True(
                fridgeModels.Count == 3 
                && fridgeModels.Any(fm => fm.Title == "Mi-270" && fm.Year ==2005) 
                && fridgeModels.Any(fm => fm.Title == "LG-220" && fm.Year == 2006) 
                && fridgeModels.Any(fm => fm.Title == "ZELMA-070" && fm.Year == 2005)
                );
        }
    } 
}
