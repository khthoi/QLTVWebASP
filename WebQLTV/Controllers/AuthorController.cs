using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLTV.Data;
using WebQLTV.Models;
using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebQLTV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AuthorDetails(int? page)
        {
            // Khởi tạo số trang mặc định là 1
            int pageNumber = page ?? 1;

            // Số lượng items trên mỗi trang
            int pageSize = 10;

            // Lấy tất cả tác giả từ database
            var authors = await _context.Authors.OrderBy(a => a.AuthorID).ToListAsync();

            // Chuyển list thành PagedList và truyền vào view
            return View("~/Views/Admin/AuthorDetails.cshtml", authors.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Tác giả đã được thêm thành công!";
            }
            return RedirectToAction("AuthorDetails");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Authors.Update(author);
                await _context.SaveChangesAsync();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Tác giả đã được cập nhật thành công!";
            }
            return RedirectToAction("AuthorDetails");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAuthor(int AuthorID)
        {
            var author = await _context.Authors.FindAsync(AuthorID);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Tác giả đã được xóa thành công!";
            }
            return RedirectToAction("AuthorDetails");
        }
    }
}
