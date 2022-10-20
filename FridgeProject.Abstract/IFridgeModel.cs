using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IFridgeModel
    {
        public  Task<List<FridgeModel>> GetFridgeModels();
        public  Task<FridgeModel> GetFridgeModelById(Guid id);
        public  Task AddFridgeModel(FridgeModel fridgeModel);
        public  Task UpdateFridgeModel(FridgeModel updatedFridgeModel);
        public  Task DeleteFridgeModel(FridgeModel fridgeModel);
    }
}
