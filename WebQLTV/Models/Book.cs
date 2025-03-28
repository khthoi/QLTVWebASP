using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQLTV.Models   // Sửa namespace nếu bị lệch
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string Title { get; set; }
        public int TypeID { get; set; }
        public BookType? Type { get; set; }
        public int AuthorID { get; set; }
        public Author? Author { get; set; }
        public int PublisherID { get; set; }
        public string ImgPath { get; set; }
        public BookPublisher? Publisher { get; set; }  // Đổi kiểu thành BookPublisher
        public string? Description { get; set; }
        [Column("total_borrow")]  // Ánh xạ cột trong CSDL
        public int TotalBorrow { get; set; }
        public int Quantity { get; set; }
    }

    

    
}
