using EducationSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Создаем базу данных, если она еще не создана
        context.Database.EnsureCreated();
        
        // Добавляем тестовые данные, если база пуста
        if (!context.Teachers.Any())
        {
            var teacher1 = new Teacher { FirstName = "Иван", LastName = "Иванов", Email = "ivanov@example.com" };
            var teacher2 = new Teacher { FirstName = "Петр", LastName = "Петров", Email = "petrov@example.com" };
            
            context.Teachers.AddRange(teacher1, teacher2);
            
            var student1 = new Student { FirstName = "Алексей", LastName = "Сидоров", Email = "sidorov@example.com" };
            var student2 = new Student { FirstName = "Мария", LastName = "Иванова", Email = "ivanova@example.com" };
            var student3 = new Student { FirstName = "Сергей", LastName = "Сергеев", Email = "sergeev@example.com" };
            
            context.Students.AddRange(student1, student2, student3);
            
            // Сначала сохраняем изменения, чтобы получить ID для преподавателей
            await context.SaveChangesAsync();
            
            var course1 = new Course { Title = "Программирование на C#", Description = "Базовый курс по C#", TeacherId = teacher1.Id };
            var course2 = new Course { Title = "Веб-разработка", Description = "Современная веб-разработка", TeacherId = teacher2.Id };
            
            context.Courses.AddRange(course1, course2);
            
            // Сохраняем курсы, чтобы получить их ID
            await context.SaveChangesAsync();
            
            // Записываем студентов на курсы
            course1.Students.Add(student1);
            course1.Students.Add(student2);
            course2.Students.Add(student2);
            course2.Students.Add(student3);
            
            await context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при инициализации базы данных");
    }
}

app.Run();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
