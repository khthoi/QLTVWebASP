using System.ComponentModel.DataAnnotations;

namespace WebQLTV.Models
{
    public class BookBorrow
    {
        [Key]
        public int BorrowID { get; set; }    // Mã phiếu mượn
        public int UserID { get; set; }      // ID người dùng (liên kết tới bảng user)
        public int BookID { get; set; }      // ID sách (liên kết tới bảng book)
        public DateTime BorrowDate { get; set; }  // Ngày mượn
        public DateTime ReturnDate { get; set; }  // Ngày trả
        public string Status { get; set; }      
    }

    public class BookBorrowViewModel
    {
        public int BorrowID { get; set; }
        public string Username { get; set; }  // Tên người mượn
        public string Title { get; set; }     // Tên sách
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }     
    }

}
