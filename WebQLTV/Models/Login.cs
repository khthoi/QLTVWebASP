using System.ComponentModel.DataAnnotations;

namespace WebQLTV.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
    }
}
