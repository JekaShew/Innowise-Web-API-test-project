using System;
using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Web.Client.Models
{
    public class SelectedProduct
    {
        [Required]
        public Guid FridgeId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [
            Required,
            Range(0,100)
        ]
        public int Quantity { get; set; }
    }
}
