using FridgeProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FridgeProject.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<Fridge> Fridges{ get; set; }

        public DbSet<FridgeModel> FridgeModels { get; set; }

        public DbSet<FridgeProduct> FridgeProducts{ get; set; }

        public DbSet<Product> Products{ get; set; }
                
        public DbSet<User> Users { get; set; }  

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public void Seed()
        {
            if (Fridges.Any())
                return;

            var fridgeModels = new[]
            {
                new FridgeModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Bosh 2007",
                    Year = 2007
                },

                new FridgeModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Bosh 2017Pro",
                    Year = 2018
                },

                new FridgeModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Atlant 2011",
                    Year = 2012
                },

                new FridgeModel
                {
                    Id = Guid.NewGuid(),
                    Title = "LG 2010",
                    Year = 2010
                },

                new FridgeModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Samsung 2012",
                    Year = 2012
                },

                new FridgeModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Xiaomi 2015",
                    Year = 2014
                },
            };

            var fridges = new[]
            {
                new Fridge
                {
                    Id = Guid.NewGuid(),
                    Title = "Best for it's money",
                    OwnerName = "Anikeev Genady Eugenevich",
                    FridgeModelId = fridgeModels[5].Id
                },

                 new Fridge
                 {
                    Id = Guid.NewGuid(),
                    Title = "Xiaomifridge",
                    OwnerName = "Markulov Kirill Pavlovich",
                    FridgeModelId = fridgeModels[5].Id
                },

                new Fridge
                {
                    Id = Guid.NewGuid(),
                    Title = "Breacking fridge",
                    OwnerName = "Nevezuchy Pavel Igorevich",
                    FridgeModelId = fridgeModels[4].Id
                },

                new Fridge
                {
                    Id = Guid.NewGuid(),
                    Title = "Smart fridge",
                    OwnerName = "Smartin Nikolai Igorevich",
                    FridgeModelId = fridgeModels[3].Id
                },

                new Fridge
                {
                    Id = Guid.NewGuid(),
                    Title = "Native fridge",
                    OwnerName = "Honest Ivan Ivanovich",
                    FridgeModelId = fridgeModels[2].Id
                },

                new Fridge
                {
                    Id = Guid.NewGuid(),
                    Title = "Such a good and big Bosh",
                    OwnerName = "Stepanuk Kirill Ivanovich",
                    FridgeModelId = fridgeModels[1].Id
                },

                new Fridge
                {
                    Id = Guid.NewGuid(),
                    Title = "Expensive fridge with products",
                    OwnerName = "Petrikov Vasiliy Vladimirovich",
                    FridgeModelId = fridgeModels[0].Id
                    
                }            
            };

            var products = new[]
            {
                new Product
                { 
                    Id = Guid.NewGuid(),
                    Title = "Cucumber",
                    DefaultQuantity = 2
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Tomato",
                    DefaultQuantity = 4
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Milk",
                    DefaultQuantity = 1
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Cheese",
                    DefaultQuantity = 1
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Sauce",
                    DefaultQuantity = 2
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Pork",
                    DefaultQuantity = 2
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Sausage",
                    DefaultQuantity = 2
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Beer",
                    DefaultQuantity = 5
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Whiskey",
                    DefaultQuantity = 2
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Vodka",
                    DefaultQuantity = 1
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Vine",
                    DefaultQuantity = 5
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Energy drink",
                    DefaultQuantity = 3
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Water",
                    DefaultQuantity = 2
                },

                new Product
                {
                    Id = Guid.NewGuid(),
                    Title = "Pepper",
                    DefaultQuantity = 3
                },
            };

            var fridgeProducts = new[]
                {
                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridges[0].Id,
                        Quantity = 4
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridges[3].Id,
                        Quantity = 2
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridges[5].Id,
                        Quantity = 14
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridges[2].Id,
                        Quantity = 1
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridges[4].Id,
                        Quantity = 7
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[13].Id,
                        FridgeId = fridges[0].Id,
                        Quantity = 6
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[4].Id,
                        FridgeId = fridges[0].Id,
                        Quantity = 5
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[7].Id,
                        FridgeId = fridges[0].Id,
                        Quantity = 8
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[8].Id,
                        FridgeId = fridges[0].Id,
                        Quantity = 1
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[2].Id,
                        FridgeId = fridges[1].Id,
                        Quantity = 8
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[5].Id,
                        FridgeId = fridges[1].Id,
                        Quantity = 8
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[9].Id,
                        FridgeId = fridges[1].Id,
                        Quantity = 9
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[10].Id,
                        FridgeId = fridges[1].Id,
                        Quantity = 19
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridges[2].Id,
                        Quantity = 10
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[12].Id,
                        FridgeId = fridges[2].Id,
                        Quantity = 5
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[11].Id,
                        FridgeId = fridges[2].Id,
                        Quantity = 4
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[7].Id,
                        FridgeId = fridges[2].Id,
                        Quantity = 12
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[4].Id,
                        FridgeId = fridges[3].Id,
                        Quantity = 8
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[2].Id,
                        FridgeId = fridges[3].Id,
                        Quantity = 48
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[1].Id,
                        FridgeId = fridges[3].Id,
                        Quantity = 1
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[0].Id,
                        FridgeId = fridges[3].Id,
                        Quantity = 3
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[3].Id,
                        FridgeId = fridges[4].Id,
                        Quantity = 7
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[4].Id,
                        FridgeId = fridges[4].Id,
                        Quantity = 7
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[5].Id,
                        FridgeId = fridges[4].Id,
                        Quantity = 7
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[6].Id,
                        FridgeId = fridges[4].Id,
                        Quantity = 2
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[7].Id,
                        FridgeId = fridges[4].Id,
                        Quantity = 5
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[8].Id,
                        FridgeId = fridges[4].Id,
                        Quantity = 10
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[9].Id,
                        FridgeId = fridges[5].Id,
                        Quantity = 16
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[10].Id,
                        FridgeId = fridges[5].Id,
                        Quantity = 7
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[11].Id,
                        FridgeId = fridges[5].Id,
                        Quantity = 10
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[12].Id,
                        FridgeId = fridges[5].Id,
                        Quantity = 5
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[13].Id,
                        FridgeId = fridges[5].Id,
                        Quantity = 4
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[11].Id,
                        FridgeId = fridges[6].Id,
                        Quantity = 2
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[4].Id,
                        FridgeId = fridges[6].Id,
                        Quantity = 3
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[9].Id,
                        FridgeId = fridges[6].Id,
                        Quantity = 11
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[1].Id,
                        FridgeId = fridges[6].Id,
                        Quantity = 8
                    },

                    new FridgeProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[2].Id,
                        FridgeId = fridges[6].Id,
                        Quantity = 4
                    }
                };

            var users = new[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Client",
                    Password = "Client123",
                    Role = Abstract.Data.Role.Client
                },

                new User
                {
                    Id = Guid.NewGuid(),
                    Login = "Admin",
                    Password = "qwerty",
                    Role = Abstract.Data.Role.Admin
                }
            };

            AddRange(users);
            AddRange(fridgeModels);
            AddRange(fridges);
            AddRange(products);
            AddRange(fridgeProducts);

            SaveChanges();
        }
    }
}
