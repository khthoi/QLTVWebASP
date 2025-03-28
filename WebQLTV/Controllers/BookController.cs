using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLTV.Data;
using WebQLTV.Models;

namespace WebQLTV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly ApplicationDbContext _context;

        public BookController(ILogger<BookController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Hiển thị danh sách sách (GET)
        public async Task<IActionResult> BookDetails()
        {
            try
            {
                var books = await _context.Books
                    .Include(b => b.Type)
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .ToListAsync();

                // Lấy danh sách file ảnh từ thư mục wwwroot/lib/img/imgbook/
                var imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lib/img/imgbook");
                var imageFiles = Directory.GetFiles(imgFolderPath)
                                          .Select(Path.GetFileName)
                                          .ToList();

                ViewData["Types"] = await _context.BookTypes.ToListAsync();
                ViewData["Authors"] = await _context.Authors.ToListAsync();
                ViewData["Publishers"] = await _context.Publishers.ToListAsync();
                ViewData["ImageFiles"] = imageFiles;  // Truyền danh sách ảnh vào ViewData

                return View("~/Views/Admin/BookDetails.cshtml", books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book details.");
                return StatusCode(500, "An error occurred while retrieving book details.");
            }
        }


        // Tạo sách mới (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Sách đã được thêm thành công!";
                    TempData["AlertType"] = "success";  // Thông báo thành công
                    return RedirectToAction("BookDetails");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating book.");
                    TempData["Message"] = "Có lỗi xảy ra khi thêm sách. Vui lòng thử lại.";
                    TempData["AlertType"] = "danger";  // Thông báo lỗi
                }
            }

            return await ReloadBookView();  // Tải lại danh sách sách nếu có lỗi
        }

        // Chỉnh sửa sách (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _context.Books.FindAsync(book.BookID);
                    if (existingBook == null)
                    {
                        TempData["Message"] = "Không tìm thấy sách để chỉnh sửa.";
                        TempData["AlertType"] = "warning";  // Cảnh báo không tìm thấy sách
                        return RedirectToAction("BookDetails");
                    }

                    // Cập nhật từng thuộc tính thay vì ghi đè toàn bộ
                    existingBook.Title = book.Title;
                    existingBook.Quantity = book.Quantity;
                    existingBook.TypeID = book.TypeID;
                    existingBook.AuthorID = book.AuthorID;
                    existingBook.PublisherID = book.PublisherID;
                    existingBook.Description = book.Description;
                    existingBook.ImgPath = book.ImgPath;

                    // Save changes
                    _context.Books.Update(existingBook);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Sách đã được cập nhật thành công!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction("BookDetails");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Có lỗi xảy ra khi chỉnh sửa sách.");
                    TempData["Message"] = "Có lỗi xảy ra khi chỉnh sửa sách.";
                    TempData["AlertType"] = "danger";
                }
            }

            // Nếu ModelState không hợp lệ, quay về trang chỉnh sửa và giữ lại dữ liệu
            TempData["Message"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
            TempData["AlertType"] = "danger";
            return RedirectToAction("BookDetails");
        }


        // Xóa sách (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    TempData["Message"] = "Không tìm thấy sách để xóa.";
                    TempData["AlertType"] = "warning";  // Cảnh báo không tìm thấy sách
                    return RedirectToAction("BookDetails");
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Sách đã được xóa thành công!";
                TempData["AlertType"] = "success";
                return RedirectToAction("BookDetails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book.");
                TempData["Message"] = "Có lỗi xảy ra khi xóa sách.";
                TempData["AlertType"] = "danger";

                // Trả về RedirectToAction ngay cả khi gặp lỗi
                return RedirectToAction("BookDetails");
            }
        }


        // Helper method để tải lại View với danh sách sách
        private async Task<IActionResult> ReloadBookView()
        {
            var books = await _context.Books
                .Include(b => b.Type)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToListAsync();

            ViewData["Types"] = await _context.BookTypes.ToListAsync();
            ViewData["Authors"] = await _context.Authors.ToListAsync();
            ViewData["Publishers"] = await _context.Publishers.ToListAsync();

            // Trả về View từ đường dẫn tĩnh
            return View("~/Views/Admin/BookDetails.cshtml", books);
        }

        // Thêm ảnh bìa sách (POST)
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lib/img/imgbook", imageFile.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                TempData["Message"] = "Ảnh đã được upload thành công!";
                TempData["AlertType"] = "success";  // Phân loại thành công
            }
            else
            {
                TempData["Message"] = "Vui lòng chọn file ảnh hợp lệ.";
                TempData["AlertType"] = "danger";  // Phân loại lỗi
            }

            return RedirectToAction("BookDetails");
        }
    }
}
