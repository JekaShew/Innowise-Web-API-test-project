using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
