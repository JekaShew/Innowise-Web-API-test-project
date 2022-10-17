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
        [Fact]
        public async void AddFridgeTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Add").Options;
            using var dbContext = new AppDBContext(options);
            var fridgeModelService = new FridgeModelServices(dbContext);
            var service = new FridgeServices(dbContext);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-770" && x.Year == 2005));
            var fridgeModel = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.First().Id);          

            await service.AddFridge(new Fridge()
            { 
                Title="����������� � �����", 
                OwnerName ="������� �������� �������", 
                FridgeModel = fridgeModel, 
                FridgeProducts = new List<FridgeProduct>() 
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � �����" && x.OwnerName == "������� �������� �������"));
        }
        
        [Fact]
        public async void GetByIdFridgeTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("GetById").Options;
            using var dbContext = new AppDBContext(options);
            var fridgeModelService = new FridgeModelServices(dbContext);
            var service = new FridgeServices(dbContext);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-770" && x.Year == 2005));
            var fridgeModel = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.First().Id);

            await service.AddFridge(new Fridge()
            {
                Title = "����������� � �����",
                OwnerName = "������� �������� �������",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � �����" && x.OwnerName == "������� �������� �������"));

            await service.AddFridge(new Fridge()
            {
                Title = "����������� � ������",
                OwnerName = "���� �������� ������",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � ������" && x.OwnerName == "���� �������� ������"));

            var fridge = await service.GetFridgeById(dbContext.Fridges.FirstOrDefault(f => f.Title == "����������� � �����").Id);

            Assert.True(fridge.Title == "����������� � �����" 
                && fridge.OwnerName == "������� �������� �������"
                && fridge.FridgeModel.Title == fridgeModel.Title);
        }

        [Fact]
        public async void DeleteFridgeTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Delete").Options;
            using var dbContext = new AppDBContext(options);
            var fridgeModelService = new FridgeModelServices(dbContext);
            var service = new FridgeServices(dbContext);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-770" && x.Year == 2005));
            var fridgeModel = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.First().Id);

            await service.AddFridge(new Fridge()
            {
                Title = "����������� � �����",
                OwnerName = "������� �������� �������",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � �����" && x.OwnerName == "������� �������� �������"));

            await service.DeleteFridge(await service.GetFridgeById(dbContext.Fridges.First().Id));

            Assert.False(dbContext.Fridges.Any(f => f.Title == "����������� � �����" && f.OwnerName == "������� �������� �������" && f.FridgeModelId == fridgeModel.Id));
        }  
              
        [Fact]
        public async void UpdateFridgeTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("Update").Options;
            using var dbContext = new AppDBContext(options);
            var fridgeModelService = new FridgeModelServices(dbContext);
            var productService = new ProductServices(dbContext);
            var service = new FridgeServices(dbContext);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-770" && x.Year == 2005));
            var fridgeModel1 = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "LG-770").Id);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "ZELMA-70",
                Year = 2011
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "ZELMA-70" && x.Year == 2011));

            await productService.AddProduct(new Product { Title = "������", DefaultQuantity = 3 });
            await productService.AddProduct(new Product { Title = "�������", DefaultQuantity = 2 });
            

            var products = await productService.GetProducts();

            var fridgeModel2 = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "ZELMA-70").Id);
            await service.AddFridge(new Fridge()
            {
                Title = "����������� � �����",
                OwnerName = "������� �������� �������",
                FridgeModel = fridgeModel1,
                FridgeProducts = new List<FridgeProduct>
                {
                    new FridgeProduct
                    {
                        Id = Guid.Parse("e0922760-07e5-4a66-86ec-2e0b597650dc"),
                        Product = products[0],
                        Quantity = 4
                    },
                     new FridgeProduct
                    {
                        Id = Guid.Parse("a47347b0-9212-46d7-921a-ff89d87d1ea1"),
                        Product = products[1],
                        Quantity = 0
                    }
                    
                }
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � �����" && x.OwnerName == "������� �������� �������"));

            await service.UpdateFridge( new Fridge
            {
                Id = dbContext.Fridges.FirstOrDefault().Id,
                Title = "����� ���� �����������",
                OwnerName = "�������� �������� ����������",
                FridgeModel = fridgeModel2,
                FridgeProducts = (await service.GetFridgeById(dbContext.Fridges.FirstOrDefault().Id)).FridgeProducts
            });

            Assert.True(
                dbContext.Fridges.Any(f => 
                    f.Title == "����� ���� �����������"
                    && f.OwnerName == "�������� �������� ����������"
                    && f.FridgeModelId == fridgeModel2.Id)
                && !dbContext.Fridges.Any(f => 
                    f.Title == "����������� � �����"
                    && f.OwnerName == "������� �������� �������"
                    && f.FridgeModelId == fridgeModel1.Id)
                );
        }

        [Fact]
        public async void GetAllFridgesTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("GetAllFridges").Options;
            using var dbContext = new AppDBContext(options);
            var fridgeModelService = new FridgeModelServices(dbContext);
            var service = new FridgeServices(dbContext);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-770" && x.Year == 2005));
            var fridgeModel1 = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "LG-770").Id);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "ZELMA-70",
                Year = 2011
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "ZELMA-70" && x.Year == 2011));
            var fridgeModel2 = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.FirstOrDefault(fm => fm.Title == "ZELMA-70").Id);

            await service.AddFridge(new Fridge()
            {
                Title = "����������� � �����",
                OwnerName = "������� �������� �������",
                FridgeModel = fridgeModel1,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � �����" && x.OwnerName == "������� �������� �������"));

            await service.AddFridge(new Fridge()
            {
                Title = "����������� � ������",
                OwnerName = "���� �������� ��������",
                FridgeModel = fridgeModel2,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � ������" && x.OwnerName == "���� �������� ��������"));

            await service.AddFridge(new Fridge()
            {
                Title = "����������� � �����������",
                OwnerName = "���� ��������� ��������",
                FridgeModel = fridgeModel1,
                FridgeProducts = new List<FridgeProduct>()
            });

            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � �����������" && x.OwnerName == "���� ��������� ��������"));

            var fridges = await service.GetFridges();

            Assert.True(
                fridges.Count == 3
                && fridges.Any(f1 => 
                    f1.Title == "����������� � �����"
                    && f1.OwnerName == "������� �������� �������"
                    && f1.FridgeModel.Title == fridgeModel1.Title)
                && fridges.Any(f2 =>
                    f2.Title == "����������� � ������"
                    && f2.OwnerName == "���� �������� ��������"
                    && f2.FridgeModel.Title == fridgeModel2.Title)
                && fridges.Any( f3 =>
                    f3.Title == "����������� � �����������"
                    && f3.OwnerName == "���� ��������� ��������"
                    && f3.FridgeModel.Title == fridgeModel1.Title)
                );
        }

        [Fact]
        public async void UpdateFridgeProductsQuantityTest()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>().UseInMemoryDatabase("UpdateQuantity").Options;
            using var dbContext = new AppDBContext(options);
            var fridgeModelService = new FridgeModelServices(dbContext);
            var productService = new ProductServices(dbContext);
            var service = new FridgeServices(dbContext);

            await fridgeModelService.AddFridgeModel(new FridgeModel()
            {
                Title = "LG-770",
                Year = 2005
            });

            Assert.True(dbContext.FridgeModels.Any(x => x.Title == "LG-770" && x.Year == 2005));
            var fridgeModel = await fridgeModelService.GetFridgeModelById(dbContext.FridgeModels.First().Id);

            await productService.AddProduct( new Product { Title = "������", DefaultQuantity = 3 });
            await productService.AddProduct(new Product { Title = "�������", DefaultQuantity = 2 });
            await productService.AddProduct(new Product { Title = "�������", DefaultQuantity = 1 });

            var products = await productService.GetProducts();
            await service.AddFridge(new Fridge()
            {
                Title = "����������� � �����",
                OwnerName = "������� �������� �������",
                FridgeModel = fridgeModel,
                FridgeProducts = new List<FridgeProduct>
                {
                    new FridgeProduct
                    {
                        Id = Guid.Parse("e0922760-07e5-4a66-86ec-2e0b597650dc"),
                        Product = products[0],
                        Quantity = 4
                    },
                     new FridgeProduct
                    {
                        Id = Guid.Parse("a47347b0-9212-46d7-921a-ff89d87d1ea1"),
                        Product = products[1],
                        Quantity = 0
                    },
                      new FridgeProduct
                    {
                        Id = Guid.Parse("0c4a00ae-2da2-49f6-a9f8-3c6d76ccf311"),
                        Product = products[2],       
                        Quantity = 4
                    }
                }
            });
        
            Assert.True(dbContext.Fridges.Any(x => x.Title == "����������� � �����" && x.OwnerName == "������� �������� �������"));

           var updatedFridges = await service.GetUpdatedFridgesWithoutQuantity();

            Assert.True(
                updatedFridges.Count == 1
                && updatedFridges.Any(uf =>
                    uf.Title == "����������� � �����"
                    && uf.OwnerName == "������� �������� �������"
                    && uf.FridgeModel.Title == fridgeModel.Title
                    && !uf.FridgeProducts.Any(fp => fp.Quantity == 0)
                ));
        }
    }
}
