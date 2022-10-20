using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract.Data
{
    public class Fridge
    {
        public Guid Id { get; set; }
        [Required,MinLength(3), MaxLength(75)]
        public string Title { get; set; }
        [MaxLength(60)]
        public string OwnerName { get; set; }
        public FridgeModel FridgeModel { get; set; }
        public List<FridgeProduct> FridgeProducts { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Fridge other && Id == other.Id;
        }
    }
}
