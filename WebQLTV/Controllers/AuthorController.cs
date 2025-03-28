using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLTV.Data;
using WebQLTV.Models;
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

        public async Task<IActionResult> AuthorDetails()
        {
            var authors = await _context.Authors.ToListAsync();
            return View("~/Views/Admin/AuthorDetails.cshtml", authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
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
                TempData["Message"] = "Tác giả đã được xóa thành công!";
            }
            return RedirectToAction("AuthorDetails");
        }
    }
}
