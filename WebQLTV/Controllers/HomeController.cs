using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQLTV.Data;
using WebQLTV.Models;

namespace WebQLTV.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : base(context)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["TypeList"] = _context.BookTypes.Select(t => new { t.TypeID, t.TypeName }).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> BookList(string searchTitle = "")
        {
            var books = string.IsNullOrEmpty(searchTitle) ?
                await _context.Books
                    .Include(b => b.Type)
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .ToListAsync()
                :
                await _context.Books
                    .Include(b => b.Type)
                    .Include(b => b.Author)
                    .Include(b => b.Publisher)
                    .Where(b => b.Title.Contains(searchTitle))
                    .ToListAsync();

            return View(books ?? new List<Book>());
        }

        public async Task<IActionResult> BooksByType(int typeID)
        {
            var books = await _context.Books
                .Include(b => b.Type)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Where(b => b.TypeID == typeID)
                .ToListAsync();

            var typeName = await _context.BookTypes
                .Where(t => t.TypeID == typeID)
                .Select(t => t.TypeName)
                .FirstOrDefaultAsync();

            ViewBag.TypeName = typeName ?? "Thể loại không xác định";

            return View("BookByType", books ?? new List<Book>());
        }

        public async Task<IActionResult> AuthorList(string searchName = "")
        {
            var authors = string.IsNullOrEmpty(searchName) ?
                await _context.Authors
                    .Include(a => a.Books)
                    .ToListAsync()
                :
                await _context.Authors
                    .Include(a => a.Books)
                    .Where(a => a.AuthorName.Contains(searchName))
                    .ToListAsync();

            return View(authors ?? new List<Author>());
        }

        public async Task<IActionResult> BookDetails(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Type)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.BookID == id);

            if (book == null) return NotFound();

            return View(book);
        }

        public IActionResult BookRanking()
        {
            // Truy vấn top 10 sách có TotalBorrow cao nhất, sắp xếp giảm dần
            // Truy vấn top 10 sách có TotalBorrow cao nhất, kèm Author và Type
            var topBooks = _context.Books
                .Include(b => b.Author)  // Tải dữ liệu bảng Author
                .Include(b => b.Type)    // Tải dữ liệu bảng Type
                .OrderByDescending(b => b.TotalBorrow)
                .Take(10)
                .ToList();

            return View("/Views/Home/BookRanking.cshtml", topBooks);
        }

        // Action mượn sách
        [HttpPost]
        [Authorize(Roles = "User")]
        public IActionResult BookBorrow(int UserID, int BookID, DateTime BorrowDate)
        {
            // Tính ngày trả mặc định là 10 ngày sau ngày mượn
            DateTime ReturnDate = BorrowDate.AddDays(10);

            // Lấy thông tin sách từ database
            var book = _context.Books.FirstOrDefault(b => b.BookID == BookID);

            if (book != null)
            {
                // Thêm thông tin phiếu mượn
                BookBorrow bookBorrow = new BookBorrow
                {
                    UserID = UserID,
                    BookID = BookID,
                    BorrowDate = BorrowDate,
                    ReturnDate = ReturnDate,
                    Status = "Pending"
                };
                _context.BookBorrow.Add(bookBorrow);

                // Tăng TotalBorrow lên 1
                book.TotalBorrow += 1;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                TempData["BorrowSuccess"] = "Bạn đã đăng ký mượn sách thành công!";
            }
            else
            {
                TempData["BorrowError"] = "Lỗi, không thể đăng ký mượn sách.";
            }

            return RedirectToAction("BookDetails", new { id = BookID });
        }


        // Action trả sách
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            var borrow = await _context.BookBorrow
                .AsNoTracking()  // Ngăn EF theo dõi thực thể
                .FirstOrDefaultAsync(b => b.BorrowID == borrowId);

            if (borrow != null && borrow.Status == "Approved")
            {
                // Lấy thông tin sách từ BookID trong borrow
                var book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == borrow.BookID);

                if (book != null)
                {
                    // Log giá trị TotalBorrow trước khi lưu
                    Console.WriteLine($"Before: TotalBorrow = {book.TotalBorrow}");

                    // Cập nhật trạng thái phiếu mượn
                    borrow.Status = "Returned";
                    _context.BookBorrow.Update(borrow);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Log giá trị TotalBorrow sau khi lưu
                    Console.WriteLine($"After: TotalBorrow = {book.TotalBorrow}");

                    TempData["Success"] = "Bạn đã trả sách thành công!";
                }
                else
                {
                    TempData["Error"] = "Không thể tìm thấy thông tin sách.";
                }
            }
            else
            {
                TempData["Error"] = "Không thể tìm thấy phiếu mượn hoặc sách đã được trả.";
            }

            return View("/Views/UserLogin/Customer/UserInfo.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
