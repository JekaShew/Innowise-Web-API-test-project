using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract.Data
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required,MinLength(3), MaxLength(65)]
        public string Title { get; set; }
        [Range(0,50)]
        public int? DefaultQuantity { get; set; }

        public List<FridgeProduct> FridgeProducts { get; set;}

        public override bool Equals(object obj)
        {
            var item = obj as Product;

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
