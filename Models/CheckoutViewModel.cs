using System.ComponentModel.DataAnnotations;

namespace CetStudentBook.Models
{
    
    
    public class CheckoutViewModel
    {
        public int BookId { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }


        [Required]
        public string City { get; set; }
        
        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string CardHolderName { get; set; }

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        public string ExpirationDate { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CVV { get; set; }
    }
}