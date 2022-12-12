using FridgeProject.Abstract.Data;
using System.Collections.Generic;

namespace FridgeProject.Web.Client.Models
{
    public class FridgeWithExistedProducts
    {
        public Fridge Fridge { get; set; }

        public List<FridgeProduct> ExistedProducts { get; set; }
    }
}
