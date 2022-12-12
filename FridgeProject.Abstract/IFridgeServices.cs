using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IFridgeServices
    {
        public Task<List<Fridge>> TakeFridges();

        public Task<Fridge> TakeFridgeById(Guid id);

        public Task AddFridge(Fridge fridge);

        public Task DeleteFridge(Guid id);

        public Task UpdateFridge(Fridge updatedFridge);

        public Task UpdateFridgeProductsWithoutQuantity();

        public Task<List<Fridge>> TakeUpdatedFridgesWithoutQuantity();
    }
}
