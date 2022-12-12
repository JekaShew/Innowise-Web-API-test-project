using FridgeProject.Abstract.Data;
using System.Collections.Generic;

namespace FridgeProject.Web.Client.Models
{
    public class FridgeWithProducts
    {
        public Fridge Fridge { get; set; }

        public List<Product> Products {get;set;}
    }
}
