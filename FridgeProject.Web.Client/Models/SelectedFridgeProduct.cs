using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FridgeProject.Web.Client.Models
{
    public class SelectedFridgeProduct
    {
        [Reqiered]
        public Guid FridgeId { get; set; }
        [Reqiered]
        public Guid FridgeProductId { get; set; }
        [Reqiered,Range(0,20)]
        public int Quantity { get; set; }
    }
}
