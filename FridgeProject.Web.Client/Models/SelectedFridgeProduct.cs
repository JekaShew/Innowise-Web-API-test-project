using System;
using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Web.Client.Models
{
    public class SelectedFridgeProduct
    {
        [Required]
        public Guid FridgeId { get; set; }

        [Required]
        public Guid FridgeProductId { get; set; }
        [
            Required,
            Range(0,20)
        ]

        public int Quantity { get; set; }
    }
}
