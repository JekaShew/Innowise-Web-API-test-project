using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IFridge
    {
        public Task<List<Fridge>> TakeFridges();
        public Task<Fridge> TakeFridgeById(Guid id);
        public Task AddFridge(Fridge fridge);
        public Task DeleteFridge(Fridge fridge);
        public Task UpdateFridge(Fridge updatedFridge);
        public Task UpdateFridgeProductsWithoutQuantity();
        public Task<List<Fridge>> TakeUpdatedFridgesWithoutQuantity();
    }
}
