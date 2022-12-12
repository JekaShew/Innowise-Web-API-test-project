using AutoMapper;
using FridgeProject.Abstract;
using FridgeProject.Abstract.Data;
using FridgeProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FridgeProject.Services
{
    public class ProductServices : IProductServices
    {
        private readonly AppDBContext _appDBContext;
        private readonly IMapper _mapper;

        public ProductServices(AppDBContext appDBContext, IMapper mapper)
        {
            _appDBContext = appDBContext;
            _mapper = mapper;
        }

        public async Task AddProduct(Product product)
        {
            var newProduct = _mapper.Map<Data.Models.Product>(product);
            await _appDBContext.Products.AddAsync(newProduct);
            await _appDBContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            _appDBContext.Products.Remove(await _appDBContext.Products.FirstOrDefaultAsync(p => p.Id == id));
            await _appDBContext.SaveChangesAsync();
        }

        public async Task<Product> TakeProductById(Guid id)
        {
            var product = await _appDBContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<Product>(product);
        }

        public async Task<List<Product>> TakeProducts()
        {
            var products = _mapper.Map<List<Product>>(await _appDBContext.Products.AsNoTracking().ToListAsync());
            return products;
        }

        public async Task UpdateProduct(Product product)
        {
            var updatedProduct = await _appDBContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            updatedProduct.Title = product.Title;
            updatedProduct.DefaultQuantity = product.DefaultQuantity;      
            await _appDBContext.SaveChangesAsync();
        }
    }

    public static class ProductServicesExtensions
    {
        public static IServiceCollection AddProductServices(this IServiceCollection services)
        {
            services.AddTransient<IProductServices, ProductServices>();
            return services;
        }
    }
}
