using Microsoft.AspNetCore.Mvc;
using WebQLTV.Models;
using WebQLTV.Data;
using System.Linq;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebQLTV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult UserDetails(int? page)
        {
            // Khởi tạo số trang mặc định là 1
            int pageNumber = page ?? 1;

            // Số lượng items trên mỗi trang
            int pageSize = 15;
            var users = _context.User.OrderBy(a => a.UserID).ToList();
            ViewData["Roles"] = _context.Roles.ToList();
            return View("~/Views/Admin/UserDetails.cshtml", users.ToPagedList(pageNumber,pageSize));
        }

        [HttpPost]
        public IActionResult CreateUser(Account user)
        {
            if (ModelState.IsValid)
            {
                _context.User.Add(user);
                _context.SaveChanges();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Thêm tài khoản thành công.";
            }
            else
            {
                TempData["Message"] = "Thêm tài khoản thất bại do ModelState không hợp lệ.";
                TempData["AlertType"] = "danger";
            }
            return RedirectToAction("UserDetails");
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng." });
            }

            TempData["EditUserID"] = user.UserID;
            TempData["EditPasswordHash"] = user.PasswordHash;
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult EditUser(string Username, string FullName, string Email, int RoleID)
        {
            var userID = (int)TempData["EditUserID"];
            var existingUser = _context.User.Find(userID);
            if (existingUser != null)
            {
                existingUser.Username = Username;
                existingUser.FullName = FullName;
                existingUser.Email = Email;
                existingUser.RoleID = RoleID;
                existingUser.PasswordHash = (string)TempData["EditPasswordHash"];

                _context.SaveChanges();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Sửa thông tin tài khoản thành công.";
            }
            else
            {
                TempData["AlertType"] = "danger";
                TempData["Message"] = "Không tìm thấy người dùng để sửa.";
            }
            return RedirectToAction("UserDetails");
        }

        [HttpPost]
        public IActionResult DeleteUser(int UserID)
        {
            var user = _context.User.Find(UserID);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Xóa tài khoản thành công.";
            }
            else
            {
                TempData["Message"] = "Xóa tài khoản thất bại do không tìm thấy người dùng.";
                TempData["AlertType"] = "danger";
            }
            return RedirectToAction("UserDetails");
        }
    }
}