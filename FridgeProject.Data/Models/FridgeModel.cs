using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Data.Models
{
    public class FridgeModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }

        public List<Fridge> Fridges { get; set; }

    }
}
