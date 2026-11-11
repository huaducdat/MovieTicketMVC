using Microsoft.EntityFrameworkCore;
using MovieTicketMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// ??ng ký MVC
builder.Services.AddControllersWithViews();

// ??c chu?i k?t n?i MySQL
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

// ??ng ký DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Trang ch? là danh sách phim
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}"
);

app.Run();