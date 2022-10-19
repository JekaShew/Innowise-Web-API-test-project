using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Models
{
    public class FridgeWithExistedProducts
    {
        public Fridge Fridge { get; set; }
        public List<FridgeProduct> ExistedProducts { get; set; }
    }
}
