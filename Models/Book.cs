using System;
using System.ComponentModel.DataAnnotations;

namespace CetStudentBook.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Kitap adı zorunludur")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Kitap adı 2-200 karakter olmalı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yazar zorunludur")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Yazar 2-200 karakter olmalı")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Yayın tarihi zorunludur")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Required(ErrorMessage = "Sayfa sayısı zorunludur")]
        [Range(1, 10000, ErrorMessage = "Sayfa sayısı 1-10000 arasında olmalı")]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "İkinci el bilgisi zorunludur")]
        public bool IsSecondHand { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}