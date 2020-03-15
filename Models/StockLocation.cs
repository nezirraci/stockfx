using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class StockLocation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nuk mund te jete emri i zbrazur")]
        public string Name { get; set; }
        public bool InHouse { get; set; }
        public bool InBase { get; set; }
        public ICollection<Stock> Stock { get; set; }
        
        }
    }

