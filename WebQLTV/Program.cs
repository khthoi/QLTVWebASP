using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebQLTV.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();  // Sử dụng Session cho TempData

// Đăng ký ApplicationDbContext với DI container và cấu hình chuỗi kết nối
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm Authentication (Cookie-based)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // Đường dẫn đến trang Login khi chưa đăng nhập
        options.Cookie.Name = "UserLoginCookie"; // Tên cookie tùy chỉnh
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Hết hạn sau 30 phút
        options.AccessDeniedPath = "/Account/AccessDenied";  // Trang bị từ chối khi không đủ quyền
    });

// Thêm Authorization (Role-based)
builder.Services.AddAuthorization(options =>
{
    // Cấu hình Role
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));  // Chỉ cho phép Admin
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));    // Chỉ cho phép User
});

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();  // Thêm Middleware Authentication
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "customer",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

app.Run();
