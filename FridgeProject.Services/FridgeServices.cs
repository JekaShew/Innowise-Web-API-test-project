using AutoMapper;
using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Services
{
    public class FridgeServices : IFridgeServices
    {
        private readonly AppDBContext _appDBContext;
        private readonly IMapper _mapper;
        public FridgeServices(AppDBContext appDBContext, IMapper mapper)
        {
            _appDBContext = appDBContext;
            _mapper = mapper;
        }

        public async Task AddFridge(Fridge fridge)
        {
            var newFridge = _mapper.Map<Data.Models.Fridge>(fridge);
            newFridge.Id = Guid.NewGuid();
            newFridge.FridgeModel = null;

            _appDBContext.FridgeProducts.AddRange(fridge.FridgeProducts.Select(fp => new Data.Models.FridgeProduct
            {
                Id = fp.Id,
                FridgeId = newFridge.Id,
                ProductId = fp.Product.Id,
                Quantity = fp.Quantity
            }));
            await _appDBContext.AddAsync(newFridge);
            await _appDBContext.SaveChangesAsync();   
        }
 
        public async Task DeleteFridge(Guid id)
        {
            _appDBContext.Fridges.Remove(await _appDBContext.Fridges.FirstOrDefaultAsync(f => f.Id == id));
            await _appDBContext.SaveChangesAsync();
        }

        public async Task<Fridge> TakeFridgeById(Guid id)
        {
            var fridge = await _appDBContext.Fridges.AsNoTracking()
                .Include(f => f.FridgeProducts)
                    .ThenInclude(fp => fp.Product)
                .Include(f => f.FridgeModel)
                .FirstOrDefaultAsync(f => f.Id == id);
            return
                 _mapper.Map<Fridge>(fridge);
        }

        public async Task<List<Fridge>> TakeFridges()
        { 
            var fridges = _mapper.Map<List<Fridge>>(await _appDBContext.Fridges.AsNoTracking()
                .Include(f => f.FridgeProducts)
                    .ThenInclude(fp => fp.Product)
                .Include(f => f.FridgeModel).ToListAsync());
            return fridges;
        }

        public async Task UpdateFridge(Fridge fridge)
        {
            var updatedFridge = await _appDBContext.Fridges.Include(x => x.FridgeProducts).FirstOrDefaultAsync(f => f.Id == fridge.Id);
            updatedFridge.Id = fridge.Id;
            updatedFridge.FridgeModel = null;
            updatedFridge.Title = fridge.Title;
            updatedFridge.OwnerName = fridge.OwnerName;
            updatedFridge.FridgeModelId = fridge.FridgeModel.Id;

            _appDBContext.FridgeProducts.RemoveRange(updatedFridge.FridgeProducts);
            var fridgeProducts = new List<Data.Models.FridgeProduct>();
            foreach (var fridgeProduct in fridge.FridgeProducts)
            {
                
                updatedFridge.FridgeProducts.Add(_mapper.Map<Data.Models.FridgeProduct>(fridgeProduct));
            }
            _appDBContext.FridgeProducts.AddRange(fridgeProducts);
            await _appDBContext.SaveChangesAsync();
        }

        public async Task UpdateFridgeProductsWithoutQuantity()
        {
            var fridgeProductWithotQuantity = await _appDBContext.FridgeProducts.FromSqlRaw($"EXECUTE dbo.SelectFridgeProductWithoutQuantity").ToListAsync(); 
            foreach(var fridgeProduct in fridgeProductWithotQuantity)
            {
                if (fridgeProduct.Product.DefaultQuantity != null)
                {
                    fridgeProduct.Quantity = fridgeProduct.Product.DefaultQuantity ?? 1; 
                }       
            }
            await _appDBContext.SaveChangesAsync();
        }

        public async Task<List<Fridge>> TakeUpdatedFridgesWithoutQuantity()
        {
            var fridgeProductWithoutQuantity = await _appDBContext.FridgeProducts.FromSqlRaw($"EXECUTE dbo.SelectFridgeProductWithoutQuantity").ToListAsync();
            foreach (var fridgeProduct in fridgeProductWithoutQuantity)
            {
                var fp = await _appDBContext.FridgeProducts
                    .Include(x => x.Product)
                    .FirstAsync(x => x.Id == fridgeProduct.Id);
                fp.Quantity = fp.Product.DefaultQuantity ?? 1;
            }
            await _appDBContext.SaveChangesAsync();

            var list = new List<Fridge>();
            foreach (var id in fridgeProductWithoutQuantity.Select(x => x.FridgeId).Distinct())
                list.Add(await TakeFridgeById(id));
            return list;
        }
    }

    public static class FridgeServicesExtensions
    {
        public static IServiceCollection AddFridgeServices(this IServiceCollection services)
        {
            services.AddTransient<IFridgeServices, FridgeServices>();
            return services;
        }
    }
}
