using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract.Data
{
   public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Login { get; set; }
       [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
