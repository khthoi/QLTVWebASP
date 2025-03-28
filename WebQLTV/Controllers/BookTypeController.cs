using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLTV.Data;
using WebQLTV.Models;

namespace WebQLTV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookTypeController> _logger;

        public BookTypeController(ApplicationDbContext context, ILogger<BookTypeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Hiển thị danh sách loại sách (GET)
        public async Task<IActionResult> BookTypeDetails()
        {
            try
            {
                var bookTypes = await _context.BookTypes.ToListAsync();
                return View("~/Views/Admin/BookTypeDetails.cshtml", bookTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book types.");
                return StatusCode(500, "Có lỗi xảy ra khi lấy danh sách loại sách.");
            }
        }

        // Thêm loại sách (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBookType(BookType bookType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.BookTypes.Add(bookType);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Loại sách đã được thêm thành công!";
                    TempData["AlertType"] = "success";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating book type.");
                    TempData["Message"] = "Có lỗi xảy ra khi thêm loại sách.";
                    TempData["AlertType"] = "danger";
                }
            }
            else
            {
                TempData["Message"] = "Dữ liệu loại sách không hợp lệ.";
                TempData["AlertType"] = "warning";
            }

            return RedirectToAction("BookTypeDetails");
        }

        // Sửa loại sách (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBookType(BookType bookType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingType = await _context.BookTypes.FindAsync(bookType.TypeID);
                    if (existingType == null)
                    {
                        TempData["Message"] = "Không tìm thấy loại sách để chỉnh sửa.";
                        TempData["AlertType"] = "warning";
                        return RedirectToAction("BookTypeDetails");
                    }

                    existingType.TypeName = bookType.TypeName;
                    _context.BookTypes.Update(existingType);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Loại sách đã được cập nhật thành công!";
                    TempData["AlertType"] = "success";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error editing book type.");
                    TempData["Message"] = "Có lỗi xảy ra khi chỉnh sửa loại sách.";
                    TempData["AlertType"] = "danger";
                }
            }
            else
            {
                TempData["Message"] = "Dữ liệu loại sách không hợp lệ.";
                TempData["AlertType"] = "warning";
            }

            return RedirectToAction("BookTypeDetails");
        }

        // Xóa loại sách (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookType(int id)
        {
            try
            {
                var bookType = await _context.BookTypes.FindAsync(id);
                if (bookType == null)
                {
                    TempData["Message"] = "Không tìm thấy loại sách để xóa.";
                    TempData["AlertType"] = "warning";
                    return RedirectToAction("BookTypeDetails");
                }

                _context.BookTypes.Remove(bookType);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Loại sách đã được xóa thành công!";
                TempData["AlertType"] = "success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book type.");
                TempData["Message"] = "Có lỗi xảy ra khi xóa loại sách.";
                TempData["AlertType"] = "danger";
            }

            return RedirectToAction("BookTypeDetails");
        }
    }
}
