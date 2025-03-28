using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace WebQLTV.Models
{
    public class Account
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string PasswordHash { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }  // Foreign Key đến Role

        // Thuộc tính Navigation (Navigation Property)
        public Role? Role { get; set; }

        [NotMapped]
        public List<BookBorrowViewModel> BorrowList { get; set; } = new List<BookBorrowViewModel>();
    }

    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }  // Tên vai trò (admin, user, ...)
    }
}
