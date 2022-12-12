using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Abstract.Data
{
    public class AuthorizationInfo
    {   
        [Required]
        public string Token { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
