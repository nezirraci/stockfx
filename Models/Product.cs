using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Nuk mund te jete emri i zbrazur")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Nuk mund te jete pershkrimi i zbrazur")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Nuk mund te jete kategoria e zbrazur")]
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<Stock> Stock { get; set; }


    }
}
