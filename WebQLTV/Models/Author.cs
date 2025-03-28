using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebQLTV.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }

        // Liên kết với bảng Book (1 tác giả có nhiều sách)
        public List<Book>? Books { get; set; }
    }
}
