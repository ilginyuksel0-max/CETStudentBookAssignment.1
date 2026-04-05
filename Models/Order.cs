using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CetStudentBook.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}