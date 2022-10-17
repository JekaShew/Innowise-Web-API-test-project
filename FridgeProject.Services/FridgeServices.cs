using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Services
{
    public class FridgeServices : IFridge
    {
        private readonly AppDBContext appDBContext;
        public FridgeServices(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }
        public async Task AddFridge(Fridge fridge)
        {
            var newFridge = new FridgeProject.Data.Models.Fridge
            {
                Id = Guid.NewGuid(),
                OwnerName = fridge.OwnerName,
                Title = fridge.Title,
                FridgeModelId = fridge.FridgeModel.Id
            };
            appDBContext.FridgeProducts.AddRange(fridge.FridgeProducts.Select(fp => new FridgeProject.Data.Models.FridgeProduct
            {
                Id = fp.Id,
                FridgeId = newFridge.Id,
                ProductId = fp.Product.Id,
                Quantity = fp.Quantity
            }));

            await appDBContext.AddAsync(newFridge);
            await appDBContext.SaveChangesAsync();
            
        }

        public async Task DeleteFridge(Fridge fridge)
        {
            var deletedFridge = await appDBContext.Fridges.Where(f => f.Id == fridge.Id).FirstOrDefaultAsync();
            appDBContext.Fridges.Remove(deletedFridge);
            await appDBContext.SaveChangesAsync();
        }

        public async Task<Fridge> GetFridgeById(Guid id)
        {
            var fridge = await appDBContext.Fridges.AsNoTracking()
                .Include(f => f.FridgeProducts)
                    .ThenInclude(fp => fp.Product)
                .Include(f => f.FridgeModel)
                .Where(f => f.Id == id).FirstOrDefaultAsync();
            return new Fridge
            {
                Id = fridge.Id,
                OwnerName = fridge.OwnerName,
                Title = fridge.Title,
                FridgeModel = new FridgeModel
                {
                    Id = fridge.FridgeModel.Id,
                    Title = fridge.FridgeModel.Title,
                    Year = fridge.FridgeModel.Year
                },
                FridgeProducts = fridge.FridgeProducts.Select(fp => new FridgeProduct
                {
                    Id = fp.Product.Id,
                    Product = new Product
                    {
                        Id = fp.Product.Id,
                        Title = fp.Product.Title,
                        DefaultQuantity = fp.Product.DefaultQuantity

                    },
                    Quantity = fp.Quantity

                }).ToList()
            };
        }

        public async Task<List<Fridge>> GetFridges()
        {
            var fridges = await appDBContext.Fridges.AsNoTracking()
                .Include(f => f.FridgeProducts)
                    .ThenInclude(fp => fp.Product)
                .Include(f => f.FridgeModel)
                .Select(f => new Fridge
                {
                    Id = f.Id,
                    Title = f.Title,
                    OwnerName = f.OwnerName,
                    FridgeModel = new FridgeModel
                    {
                        Id = f.FridgeModel.Id,
                        Title = f.FridgeModel.Title,
                        Year = f.FridgeModel.Year
                    },
                    FridgeProducts = f.FridgeProducts.Select(fp => new FridgeProduct
                    {
                        Id = fp.Id,
                        Quantity = fp.Quantity,
                        Product = new Product
                        {
                            Id = fp.Product.Id,
                            Title = fp.Product.Title,
                            DefaultQuantity = fp.Product.DefaultQuantity
                        }
                    }).ToList()
                }).ToListAsync();
            return fridges;
        }

        public async Task UpdateFridge(Fridge fridge)
        {
            var updatedFridge = await appDBContext.Fridges.Include(x => x.FridgeProducts).Where(f => f.Id == fridge.Id).FirstOrDefaultAsync();
            updatedFridge.Title = fridge.Title;
            updatedFridge.OwnerName = fridge.OwnerName;
            updatedFridge.FridgeModelId = fridge.FridgeModel.Id;

            appDBContext.FridgeProducts.RemoveRange(updatedFridge.FridgeProducts);
            appDBContext.FridgeProducts.AddRange(fridge.FridgeProducts.Select(fp => new FridgeProject.Data.Models.FridgeProduct
            {
                Id = Guid.NewGuid(),
                FridgeId = updatedFridge.Id,
                ProductId = fp.Product.Id,
                Quantity = fp.Quantity
            }));

            await appDBContext.SaveChangesAsync();
        }

        public async Task UpdateFridgeProductsWithoutQuantity()
        {
            var fridgeProductWithotQuantity = await appDBContext.FridgeProducts.FromSqlRaw($"EXECUTE dbo.SelectFridgeProductWithoutQuantity").ToListAsync(); 
            foreach(var fridgeProduct in fridgeProductWithotQuantity)
            {
                if (fridgeProduct.Product.DefaultQuantity != null)
                {
                    fridgeProduct.Quantity = fridgeProduct.Product.DefaultQuantity.Value; 
                }      
                
            }
            await appDBContext.SaveChangesAsync();
        }

        public async Task<List<Fridge>> GetUpdatedFridgesWithoutQuantity()
        {
            var fridgeProductWithotQuantity = await appDBContext.FridgeProducts.FromSqlRaw($"EXECUTE dbo.SelectFridgeProductWithoutQuantity").ToListAsync();

            foreach (var fridgeProduct in fridgeProductWithotQuantity)
            {
                var fridge = await GetFridgeById(fridgeProduct.FridgeId);
                var model = fridge.FridgeProducts.First(x => x.Product.Id == fridgeProduct.ProductId);
                model.Quantity = model.Product.DefaultQuantity ?? 0;
                await UpdateFridge(fridge);
            }

            var list = new List<Fridge>();
            foreach (var id in fridgeProductWithotQuantity.Select(x => x.FridgeId).Distinct())
                list.Add(await GetFridgeById(id));
            return list; 
        }
    }
}
