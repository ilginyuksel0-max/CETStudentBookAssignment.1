using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CetStudentBook.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        public string? PostalCode { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}