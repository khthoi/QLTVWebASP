using Microsoft.AspNetCore.Mvc;
using WebQLTV.Models;
using WebQLTV.Data;  // Namespace cho DbContext
using System.Linq;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebQLTV.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublisherController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult PublisherDetails(int? page)
        {
            // Khởi tạo số trang mặc định là 1
            int pageNumber = page ?? 1;

            // Số lượng items trên mỗi trang
            int pageSize = 10;

            var publishers = _context.Publishers.OrderBy(a => a.PublisherID).ToList();
            return View("~/Views/Admin/PublisherDetails.cshtml", publishers.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public IActionResult CreatePublisher(BookPublisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Publishers.Add(publisher);
                _context.SaveChanges();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Thêm nhà xuất bản thành công.";
            }
            return RedirectToAction("PublisherDetails");
        }

        [HttpPost]
        public IActionResult EditPublisher(BookPublisher publisher)
        {
            if (ModelState.IsValid)
            {
                var existingPublisher = _context.Publishers.Find(publisher.PublisherID);
                if (existingPublisher != null)
                {
                    existingPublisher.PublisherName = publisher.PublisherName;
                    _context.SaveChanges();
                    TempData["AlertType"] = "success";
                    TempData["Message"] = "Sửa thông tin nhà xuất bản thành công.";
                }
            }
            return RedirectToAction("PublisherDetails");
        }

        [HttpPost]
        public IActionResult DeletePublisher(int PublisherID)
        {
            var publisher = _context.Publishers.Find(PublisherID);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
                TempData["AlertType"] = "success";
                TempData["Message"] = "Xóa nhà xuất bản thành công.";
            }
            else
            {
                TempData["AlertType"] = "danger";
                TempData["Message"] = "Không tìm thấy nhà xuất bản để xóa.";
            }
            return RedirectToAction("PublisherDetails");
        }
    }
}
