using System;
using System.ComponentModel.DataAnnotations;

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
       public Role Role { get; set; }
    }
    public enum Role { Client, Admin }
}
