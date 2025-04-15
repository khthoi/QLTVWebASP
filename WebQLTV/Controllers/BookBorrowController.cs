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
    public class BookBorrowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookBorrowController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult BookborrowDetails(string searchUsername, int? page)
        {
            // Khởi tạo số trang mặc định là 1
            int pageNumber = page ?? 1;

            // Số lượng items trên mỗi trang
            int pageSize = 15;

            var borrowQuery = (from borrow in _context.BookBorrow
                               join user in _context.User on borrow.UserID equals user.UserID
                               join book in _context.Books on borrow.BookID equals book.BookID
                               select new BookBorrowViewModel
                               {
                                   BorrowID = borrow.BorrowID,
                                   Username = user.FullName, // hoặc user.Username tùy theo yêu cầu
                                   Title = book.Title,
                                   BorrowDate = borrow.BorrowDate,
                                   ReturnDate = borrow.ReturnDate,
                                   Status = borrow.Status
                               });

            // Thêm logic tìm kiếm theo tên người dùng
            if (!string.IsNullOrEmpty(searchUsername))
            {
                borrowQuery = borrowQuery.Where(b => b.Username.Contains(searchUsername));
            }

            // Lấy danh sách dữ liệu
            var borrowList = borrowQuery.ToList();

            // Lưu giá trị tìm kiếm để hiển thị lại trong view
            ViewBag.SearchUsername = searchUsername;

            // Chuyển danh sách sang dạng phân trang
            var pagedBorrowList = borrowList.ToPagedList(pageNumber, pageSize);

            return View("~/Views/Admin/BookborrowDetails.cshtml", pagedBorrowList);
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

        [HttpPost]
        public IActionResult BookBorrowDelete(int borrowId)
        {
            try
            {
                var borrow = _context.BookBorrow.FirstOrDefault(b => b.BorrowID == borrowId);
                if (borrow == null)
                {
                    TempData["BookApproveMessage"] = "Không tìm thấy phiếu mượn.";
                    TempData["BookApproveAlertType"] = "danger";
                    return RedirectToAction("BookborrowDetails");
                }

                if (borrow.Status != "Returned")
                {
                    TempData["BookApproveMessage"] = "Chỉ có thể xóa phiếu mượn đang ở trạng thái Returned.";
                    TempData["BookApproveAlertType"] = "warning";
                    return RedirectToAction("BookborrowDetails");
                }

                _context.BookBorrow.Remove(borrow);
                _context.SaveChanges();

                TempData["BookApproveMessage"] = "Phiếu mượn đã được xóa thành công!";
                TempData["BookApproveAlertType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["BookApproveMessage"] = $"Lỗi khi xóa phiếu mượn: {ex.Message}";
                TempData["BookApproveAlertType"] = "danger";
            }
            return RedirectToAction("BookborrowDetails");
        }
    }
}