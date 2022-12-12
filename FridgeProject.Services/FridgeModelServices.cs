using FridgeProject.Data;
using FridgeProject.Abstract.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FridgeProject.Abstract;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace FridgeProject.Services
{
    public class FridgeModelServices : IFridgeModelServices
    {
        private readonly AppDBContext _appDBContext;
        private readonly IMapper _mapper;

        public FridgeModelServices(AppDBContext appDBContext, IMapper mapper)
        {
            _appDBContext = appDBContext;
            _mapper = mapper;
        }

        public async Task AddFridgeModel(FridgeModel fridgeModel)
        {
            var newFridgeModel = _mapper.Map<Data.Models.FridgeModel>(fridgeModel);
            await _appDBContext.FridgeModels.AddAsync(newFridgeModel);
            await _appDBContext.SaveChangesAsync();
        }

        public async Task DeleteFridgeModel(Guid id)
        { 
            _appDBContext.FridgeModels.Remove(await _appDBContext.FridgeModels.FirstOrDefaultAsync(fm => fm.Id == id));
            await _appDBContext.SaveChangesAsync();
        }

        public async Task<List<FridgeModel>> TakeFridgeModels()
        {
            var fridgeModels = _mapper.Map<List<FridgeModel>>(await _appDBContext.FridgeModels.AsNoTracking().ToListAsync());    
            return fridgeModels;
        }

        public async Task<FridgeModel> TakeFridgeModelById(Guid id)
        {
            var selectedFridgeModel = await _appDBContext.FridgeModels.AsNoTracking().FirstAsync(fm => fm.Id == id);
            return _mapper.Map<FridgeModel>(selectedFridgeModel);
        }

        public async Task UpdateFridgeModel(FridgeModel fridgemodel)
        {
            var updatedFridgeModel = await _appDBContext.FridgeModels.FirstAsync(fm => fm.Id == fridgemodel.Id);
            updatedFridgeModel.Title = fridgemodel.Title;
            updatedFridgeModel.Year = fridgemodel.Year;
            await _appDBContext.SaveChangesAsync();
        }
    }

    public static class FridgeModelServicesExtensions
    {
        public static IServiceCollection AddFridgeModelServices(this IServiceCollection services)
        {
            services.AddTransient<IFridgeModelServices, FridgeModelServices>();
            return services;
        }
    }
}
