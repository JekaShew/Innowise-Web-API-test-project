
using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract
{
    public interface IProduct
    {
        public Task<List<Product>> GetProducts();
        public Task<Product> GetProductById(Guid id);
        public Task AddProduct(Product product);
        public Task UpdateProduct(Product updatedProduct);
        public Task DeleteProduct(Product product);

    }
}
