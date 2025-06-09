using Microsoft.EntityFrameworkCore;
using EducationSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем поддержку MVC
builder.Services.AddControllersWithViews();

// Добавляем поддержку Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Создание и инициализация базы данных при запуске
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Убедимся, что база данных создана
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при инициализации базы данных.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Настройка маршрутизации MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Роут для курсов
app.MapControllerRoute(
    name: "courses",
    pattern: "Courses/{action=Index}/{id?}",
    defaults: new { controller = "Courses" });

// Роут для студентов
app.MapControllerRoute(
    name: "students",
    pattern: "Students/{action=Index}/{id?}",
    defaults: new { controller = "Students" });

// Роут для преподавателей
app.MapControllerRoute(
    name: "teachers",
    pattern: "Teachers/{action=Index}/{id?}",
    defaults: new { controller = "Teachers" });

// Поддержка страниц Razor
app.MapRazorPages();

app.Run();
