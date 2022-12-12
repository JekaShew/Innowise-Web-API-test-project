using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Abstract.Data
{
    public class LogInInfo
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
