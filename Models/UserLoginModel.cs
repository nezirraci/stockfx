using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class UserLoginModel
    {

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Gjatesia e username duhet te jete 5-30 karaktere")]
        public string Username { get; set; }


        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Gjatesia e passwordit duhet te jete 5-30 karaktere")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

      
    }
}
