using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract.Data
{
    public class FridgeProduct
    {
        public Guid Id { get; set; }
        public Fridge Fridge { get; set; }
        public Product Product { get; set; }
        [Range(0,300)]
        public int Quantity { get; set; }
    }
}
