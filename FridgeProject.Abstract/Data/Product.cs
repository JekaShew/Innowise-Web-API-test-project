using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Abstract.Data
{
    public class Product
    {
        public Guid Id { get; set; }

        [
            Required,
            MinLength(3),
            MaxLength(65)
        ]
        public string Title { get; set; }

        [Range(0,50)]
        public int? DefaultQuantity { get; set; }
        public List<FridgeProduct> FridgeProducts { get; set;}

        public override bool Equals(object obj)
        { 
            if (obj is Product product)
            {
                return Id == product.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
