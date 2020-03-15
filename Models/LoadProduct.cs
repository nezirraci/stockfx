using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class LoadProduct
    {
        [Required(ErrorMessage ="Komenti nuk duhet te jete i zbrazte!")]
        public string Coment { get; set; }
        
        [Required(ErrorMessage = "Stoku burimor nuk duhet te jete i zbrazte!")]
        public int SourceId { get; set; }

        [Required(ErrorMessage = "Stoku i destinacionit nuk duhet te jete i zbrazte!")]
        public int DestinationId { get; set; }
        public List<Load> Loads { get; set; }
    }

  
}
