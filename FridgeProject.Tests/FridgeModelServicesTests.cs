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
    public class FridgeModelServicesTests
    {
        private (AppDBContext, FridgeModelServices) Init()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<Services.AutoMapper>());
            var mapper = config.CreateMapper();
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var dbContext = new AppDBContext(options);
            var service = new FridgeModelServices(dbContext,mapper);
            return (dbContext, service);
        }

        [Fact]
        public async void AddFridgeModel_ValidModel_DataWasSaved()
        {
            var (dbContext, service) = Init();
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-770"));
        }

        [Fact]
        public async void TakeByIdFridgeModel_ValidModel_DataExist()
        {
            var (dbContext, service) = Init();
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-170",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-170"));

            var fridgeModel = await service.TakeFridgeModelById(dbContext.FridgeModels.First().Id);

            Assert.True(fridgeModel.Title == "LG-170" && fridgeModel.Year == 2005);
        }

        [Fact]
        public async void DeleteFridgeModel_ValidModel_DataWasDeleted()
        {
            var (dbContext, service) = Init();
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-970",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync (x => x.Title == "LG-970"));

            var fridgeModel = await dbContext.FridgeModels.FirstOrDefaultAsync(fm => fm.Title == "LG-970");
            await service.DeleteFridgeModel(fridgeModel.Id);

            Assert.False(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-970"));
        }

        [Fact]
        public async void UpdateFridgeModel_ValidModel_DataWasUpdated()
        {
            var (dbContext, service) = Init();
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-970",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-970"));

            var fridgeModel = await dbContext.FridgeModels.FirstOrDefaultAsync(fm => fm.Title == "LG-970");
            await service.UpdateFridgeModel(new FridgeModel {Id = dbContext.FridgeModels.First(fm=>fm.Title == "LG-970").Id, Title = "LG-2370", Year = 2010});

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-2370" && x.Year == 2010) && !dbContext.FridgeModels.Any(x => x.Title == "LG-970" && x.Year == 2005));
        }

        [Fact]
        public async void TakeFridgeModels_ValidModels_DataExist()
        {
            var (dbContext, service) = Init();
            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "Mi-270",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync (x => x.Title == "Mi-270"));

            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-220",
                Year = 2006
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "LG-220"));

            await service.AddFridgeModel(new FridgeModel()
            {
                Title = "ZELMA-070",
                Year = 2005
            });

            Assert.True(await dbContext.FridgeModels.AnyAsync(x => x.Title == "ZELMA-070"));

            var fridgeModels = await service.TakeFridgeModels();

            Assert.True(
                fridgeModels.Count == 3 
                && fridgeModels.Any(fm => fm.Title == "Mi-270" && fm.Year ==2005) 
                && fridgeModels.Any(fm => fm.Title == "LG-220" && fm.Year == 2006) 
                && fridgeModels.Any(fm => fm.Title == "ZELMA-070" && fm.Year == 2005)
                );
        }
    } 
}
