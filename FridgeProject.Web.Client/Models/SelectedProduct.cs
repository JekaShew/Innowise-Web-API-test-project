using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Models
{
    public class SelectedProduct
    {
        [Required]
        public Guid FridgeId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required,Range(0,100)]
        public int Quantity { get; set; }
    }
}
