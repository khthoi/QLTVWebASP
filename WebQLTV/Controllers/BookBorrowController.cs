using Microsoft.AspNetCore.Mvc;
using WebQLTV.Models;
using WebQLTV.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebQLTV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookBorrowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookBorrowController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult BookborrowDetails()
        {
            var borrowList = from borrow in _context.BookBorrow
                             join user in _context.User on borrow.UserID equals user.UserID
                             join book in _context.Books on borrow.BookID equals book.BookID
                             select new BookBorrowViewModel
                             {
                                 BorrowID = borrow.BorrowID,
                                 Username = user.FullName, // hoặc user.Username
                                 Title = book.Title,
                                 BorrowDate = borrow.BorrowDate,
                                 ReturnDate = borrow.ReturnDate,
                                 Status = borrow.Status
                             };

            return View("~/Views/Admin/BookborrowDetails.cshtml", borrowList.ToList());
        }


        [HttpPost]
        public IActionResult BookBorrowApprove(int borrowId)
        {
            try
            {
                // Tìm bản ghi BookBorrow theo borrowId
                var borrow = _context.BookBorrow.FirstOrDefault(b => b.BorrowID == borrowId);

                if (borrow == null)
                {
                    TempData["BookApproveError"] = "Không tìm thấy phiếu mượn.";
                    return RedirectToAction("BookborrowDetails");
                }

                if (borrow.Status != "Pending")
                {
                    TempData["BookApproveError"] = "Chỉ có thể phê duyệt phiếu mượn đang ở trạng thái Pending.";
                    return RedirectToAction("BookborrowDetails");
                }

                // Cập nhật trạng thái sang Approved
                borrow.Status = "Approved";
                _context.Update(borrow);
                _context.SaveChanges();

                TempData["BookApproveMessage"] = "Phiếu mượn đã được phê duyệt thành công!";
                TempData["BookApproveAlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["BookApproveMessage"] = $"Lỗi khi phê duyệt phiếu mượn: {ex.Message}";
                TempData["BookApproveAlertType"] = "danger";
            }

            return RedirectToAction("BookborrowDetails");
        }
    }
}