using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class UserRegisterModel
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [StringLength(20,MinimumLength = 5, ErrorMessage = "Password should contain at least 5 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
