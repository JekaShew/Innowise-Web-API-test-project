using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Data.Models
{
    public class Fridge
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string? OwnerName { get; set; }

        [Required]
        public Guid FridgeModelId { get; set; }

        public FridgeModel FridgeModel { get; set; }
        public List<FridgeProduct> FridgeProducts { get; set; }
    }
}
