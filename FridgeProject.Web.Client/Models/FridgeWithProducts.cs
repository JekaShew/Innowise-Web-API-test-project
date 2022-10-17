using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Models
{
    public class FridgeWithProducts
    {
        public Fridge Fridge { get; set; }
        public List<Product> Products {get;set;}
    }
}
