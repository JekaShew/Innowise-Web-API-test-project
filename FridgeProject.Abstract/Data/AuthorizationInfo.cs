using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeProject.Abstract.Data
{
    public class AuthorizationInfo
    {   [Required]
        public string Token { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
