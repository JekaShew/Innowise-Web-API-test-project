using FridgeProject.Data;
using FridgeProject.Abstract.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FridgeProject.Abstract;

namespace FridgeProject.Services
{
    public class FridgeModelServices : IFridgeModel
    {
        private readonly AppDBContext appDBContext;

        public FridgeModelServices(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }
        public async Task AddFridgeModel(FridgeModel fridgeModel)
        {
            var newFridgeModel = new Data.Models.FridgeModel
            {
                Id = Guid.NewGuid(),
                Title = fridgeModel.Title,
                Year = fridgeModel.Year
            };

            await appDBContext.FridgeModels.AddAsync(newFridgeModel);
            await appDBContext.SaveChangesAsync();
        }
        
        public async Task DeleteFridgeModel(FridgeModel fridgeModel)
        {
            var deletedFridgeModel = await appDBContext.FridgeModels.Where(fm => fm.Id == fridgeModel.Id).FirstOrDefaultAsync();
            appDBContext.FridgeModels.Remove(deletedFridgeModel);
            await appDBContext.SaveChangesAsync();   
        }

        public async Task<List<FridgeModel>> TakeFridgeModels()
        {
            var fridgeModels = await appDBContext.FridgeModels.AsNoTracking()
                .Select(fm => new FridgeModel
                {
                    Id = fm.Id,
                    Title = fm.Title,
                    Year = fm.Year
                })
                .ToListAsync();
            return fridgeModels;
        }

        public async Task<FridgeModel> TakeFridgeModelById(Guid id)
        {
            var selectedFridgeModel = await appDBContext.FridgeModels.AsNoTracking().Where(fm => fm.Id == id).FirstAsync ();
            
            return new FridgeModel 
            { 
                Id = selectedFridgeModel.Id, 
                Title = selectedFridgeModel.Title, 
                Year = selectedFridgeModel.Year
            };
        }

        public async Task UpdateFridgeModel(FridgeModel fridgemodel)
        {
            var updatedFridgeModel = await appDBContext.FridgeModels.Where(fm => fm.Id == fridgemodel.Id).FirstAsync();
            updatedFridgeModel.Title = fridgemodel.Title;
            updatedFridgeModel.Year = fridgemodel.Year;

            await appDBContext.SaveChangesAsync();
        }
    }
}
