using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class ProductCategory
    {
      
        public int Id { get; set; }

        [Required(ErrorMessage ="Nuk mund te jete emri i zbrazur")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
