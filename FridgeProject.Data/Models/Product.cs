using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
            
        public int? DefaultQuantity { get; set; }

        public List<FridgeProduct> FridgeProducts { get; set; }
    }
}
