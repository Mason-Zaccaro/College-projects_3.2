using UserRegister.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Регистрируем необходимые сервисы
builder.Services.AddControllersWithViews();               // MVC + [ApiController]
builder.Services.AddSingleton<UserStorageService>();      // наш “in-memory” сервис хранения

var app = builder.Build();

// 2) Конфигурация middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();       // уместно для Production
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 3) Привязываем маршруты
// Сначала – MapControllers (обязательно для [ApiController])
app.MapControllers();

// Затем – MVC-маршрут (если нужен контроллер Register для рендеринга вьюхи)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Register}/{action=Index}/{id?}");

app.Run();
