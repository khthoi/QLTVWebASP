using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebQLTV.Data;

public class BaseController : Controller
{
    protected readonly ApplicationDbContext _context;

    public BaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Phương thức này sẽ được gọi trước mỗi action
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        // Gán TypeList cho tất cả các action
        ViewData["TypeList"] = _context.BookTypes.Select(t => new { t.TypeID, t.TypeName }).ToList();
    }
}
