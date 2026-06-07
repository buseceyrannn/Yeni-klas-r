using System.ComponentModel.DataAnnotations;

namespace BUSEB.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string CardName { get; set; }

        [Required]
        public string Expiry { get; set; }

        [Required]
        public string CVV { get; set; }
    }
}