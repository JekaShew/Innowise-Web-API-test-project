using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract.Data
{
    public class FridgeModel
    {
        public Guid Id { get; set; }
        [Required, MinLength(3), MaxLength(20)]
        public string Title { get; set; }
        [Range(1900,3000)]
        public int? Year { get; set; }
    }
}
