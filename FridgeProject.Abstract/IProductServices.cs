using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IProductServices
    {
        public Task<List<Product>> TakeProducts();

        public Task<Product> TakeProductById(Guid id);

        public Task AddProduct(Product product);

        public Task UpdateProduct(Product updatedProduct);

        public Task DeleteProduct(Guid id);
    }
}
