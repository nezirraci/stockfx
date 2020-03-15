using System.ComponentModel.DataAnnotations;

namespace ElcomManage.Models
{
    public class Load
    {
        [Required(ErrorMessage ="Produkti nuk mund te jete i zbrazte!")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Nuk mund te jete sasia number negativ ose 0")]
        [Required(ErrorMessage = "Sasia nuk mund te jete e zbrazte!")]
        public int Quantity { get; set; }
    }
}