using System;
using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Data.Models
{
    public class FridgeProduct
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid FridgeId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Product Product { get; set; }

        public Fridge Fridge { get; set; }

    }
}
