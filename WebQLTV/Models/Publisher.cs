using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebQLTV.Models
{
    public class BookPublisher
    {
        [Key]
        public int PublisherID { get; set; }
        public string PublisherName { get; set; } = string.Empty;
    }
}
