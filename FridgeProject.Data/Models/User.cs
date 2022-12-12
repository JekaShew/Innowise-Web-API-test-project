using FridgeProject.Abstract.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace FridgeProject.Data.Models
{ 
     public class User
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public Role Role { get; set; }   
    }
}
    