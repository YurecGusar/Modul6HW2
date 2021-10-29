using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Modul6HW2.Models.PostModels
{
    public class UserDetailsPostModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
