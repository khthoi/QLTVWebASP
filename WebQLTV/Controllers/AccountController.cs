using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebQLTV.Models;
using WebQLTV.Data;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity;

namespace WebQLTV.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext context) : base(context)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Kiểm tra nếu người dùng đã đăng nhập
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserInfo", "Account");
            }
            // Nếu đã đăng nhập, chuyển hướng đến trang chính
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Trả về View từ đường dẫn tùy chỉnh
            return View("/Views/UserLogin/Customer/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var account = _context.User.FirstOrDefault(u =>
                    u.Username == model.Username && u.PasswordHash == model.PasswordHash);

                if (account != null)
                {
                    // Tạo danh sách các Claim chứa thông tin người dùng
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.Username),
                        new Claim(ClaimTypes.Role, account.RoleID == 2 ? "Admin" : "User"),
                        new Claim("UserID", account.UserID.ToString()),
                        new Claim("FullName", account.FullName ?? ""),
                        new Claim("Email", account.Email ?? "")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,  // Lưu đăng nhập qua nhiều phiên làm việc
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)  // Cookie hết hạn sau 30 phút
                    };

                    // Đăng nhập người dùng với Cookie Authentication
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Điều hướng theo vai trò người dùng
                    if (account.RoleID == 2)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            // Trả về View tùy chỉnh khi đăng nhập thất bại
            return View("/Views/UserLogin/Customer/Login.cshtml");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("/Views/UserLogin/Customer/Register.cshtml");
        }

        [HttpPost]
        public IActionResult Register(string Username, string FullName, string Email, string PasswordHash)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(PasswordHash))
            {
                ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.");
                return View("/Views/UserLogin/Customer/Register.cshtml");
            }

            // Kiểm tra trùng username
            var existingUser = _context.User.FirstOrDefault(u => u.Username == Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Tên tài khoản đã tồn tại. Vui lòng chọn tên khác.");
                return View("/Views/UserLogin/Customer/Register.cshtml");
            }

            var newAccount = new Account
            {
                Username = Username,
                FullName = FullName,
                Email = Email,
                PasswordHash = PasswordHash,
                RoleID = 1
            };

            _context.User.Add(newAccount);
            _context.SaveChanges();

            TempData["Message"] = "Đăng ký thành công! Hãy đăng nhập.";
            return RedirectToAction("Login");
        }

        //[Authorize]  // Bắt buộc người dùng phải đăng nhập để truy cập UserInfo
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult UserInfo()
        {
            var username = HttpContext.User.Identity?.Name;

            var user = _context.User.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login");
            }

            // Lấy dữ liệu từ bảng BookBorrow và Book (truy vấn SQL thực sự)
            var bookBorrows = _context.BookBorrow.Where(b => b.UserID == user.UserID).ToList();
            var books = _context.Books.ToList();

            // Ánh xạ dữ liệu sang ViewModel trong bộ nhớ
            var borrowList = bookBorrows.Join(books,
                                              borrow => borrow.BookID,
                                              book => book.BookID,
                                              (borrow, book) => new BookBorrowViewModel
                                              {
                                                  BorrowID = borrow.BorrowID,
                                                  Username = user.Username,
                                                  Title = book.Title,
                                                  BorrowDate = borrow.BorrowDate,
                                                  ReturnDate = borrow.ReturnDate,
                                                  Status = borrow.Status
                                              }).ToList();

            // Gắn BorrowList vào user
            user.BorrowList = borrowList;

            // Trả về View với dữ liệu User chứa danh sách BorrowList
            return View("/Views/UserLogin/Customer/UserInfo.cshtml", user);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult BookReturn(int BorrowID)
        {
            var username = HttpContext.User.Identity?.Name;

            var user = _context.User.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login");
            }

            // Tìm phiếu mượn theo BorrowID
            var bookBorrow = _context.BookBorrow.FirstOrDefault(b => b.BorrowID == BorrowID && b.UserID == user.UserID);

            if (bookBorrow == null)
            {
                TempData["Error"] = "Không tìm thấy phiếu mượn!";
                return RedirectToAction("UserInfo");
            }

            // Thay đổi trạng thái phiếu mượn thành "Returned"
            bookBorrow.Status = "Returned";

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            TempData["Success"] = "Bạn đã trả sách thành công!";

            // Lấy lại danh sách phiếu mượn để hiển thị trong View
            var bookBorrows = _context.BookBorrow.Where(b => b.UserID == user.UserID).ToList();
            var books = _context.Books.ToList();

            var borrowList = bookBorrows.Join(books,
                                              borrow => borrow.BookID,
                                              book => book.BookID,
                                              (borrow, book) => new BookBorrowViewModel
                                              {
                                                  BorrowID = borrow.BorrowID,
                                                  Username = user.Username,
                                                  Title = book.Title,
                                                  BorrowDate = borrow.BorrowDate,
                                                  ReturnDate = borrow.ReturnDate,
                                                  Status = borrow.Status
                                              }).ToList();

            user.BorrowList = borrowList;

            return View("/Views/UserLogin/Customer/UserInfo.cshtml", user);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult Edituserinfo(int UserID, string FullName, string Email)
        {
            var user = _context.User.FirstOrDefault(u => u.UserID == UserID);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Login");
            }
            
            user.FullName = FullName;
            user.Email = Email;
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("UserInfo");
        }

        public async Task<IActionResult> Logout()
        {
            // Đăng xuất bằng cách xóa Cookie Authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
