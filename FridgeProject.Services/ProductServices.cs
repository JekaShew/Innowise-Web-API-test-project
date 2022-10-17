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
    public class ProductServices : IProduct
    {
        private readonly AppDBContext appDBContext;
        public ProductServices(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }
        public async Task AddProduct(Product product)
        {
            var newProduct = new FridgeProject.Data.Models.Product
            {
                Id = Guid.NewGuid(),
                Title = product.Title,
                DefaultQuantity = product.DefaultQuantity
            };

            await appDBContext.Products.AddAsync(newProduct);
            await appDBContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product product)
        {
            var deletedProduct = await appDBContext.Products.Where(p => p.Id == product.Id).FirstOrDefaultAsync();
            appDBContext.Products.Remove(deletedProduct);
            await appDBContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await appDBContext.Products.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            return new Product { Id = product.Id, Title = product.Title, DefaultQuantity = product.DefaultQuantity};

        }

        public async Task<List<Product>> GetProducts()
        {
            var products = await appDBContext.Products.AsNoTracking().Select(p => new Product 
                {
                    Id = p.Id,
                    Title = p.Title, 
                    DefaultQuantity = p.DefaultQuantity, 
                    FridgeProducts = p.FridgeProducts.Select(fp => new FridgeProduct 
                    { 
                        Id =fp.Id, 
                        Fridge = new Fridge
                        {
                            Id = fp.Fridge.Id,
                            OwnerName = fp.Fridge.OwnerName,
                            Title = fp.Fridge.Title,
                            FridgeModel = new FridgeModel
                            {
                                Id = fp.Fridge.FridgeModel.Id,
                                Title = fp.Fridge.FridgeModel.Title,
                                Year = fp.Fridge.FridgeModel.Year
                            }
                        }, 
                        Quantity = fp.Quantity
                    }).ToList()
                }).ToListAsync();
            return products;
        }

        public async Task UpdateProduct(Product product)
        {
            var updatedProduct = await appDBContext.Products.Where(p => p.Id == product.Id).FirstOrDefaultAsync();
            updatedProduct.Title = product.Title;
            updatedProduct.DefaultQuantity = product.DefaultQuantity;
            

            await appDBContext.SaveChangesAsync();
        
        }
    }
}
