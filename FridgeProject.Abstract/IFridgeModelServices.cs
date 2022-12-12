using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IFridgeModelServices
    {
        public  Task<List<FridgeModel>> TakeFridgeModels();

        public  Task<FridgeModel> TakeFridgeModelById(Guid id);

        public  Task AddFridgeModel(FridgeModel fridgeModel);

        public  Task UpdateFridgeModel(FridgeModel updatedFridgeModel);

        public Task DeleteFridgeModel(Guid id);
    }
}
