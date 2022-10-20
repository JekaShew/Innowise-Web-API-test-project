using FridgeProject.Abstract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Models
{
    public class FridgeWithFridgeModels
    {

        public Fridge Fridge { get; set; }
        public List<FridgeModel> FridgeModels{ get; set; }
        public Guid FridgeModelId { get; set; }
    }
}
