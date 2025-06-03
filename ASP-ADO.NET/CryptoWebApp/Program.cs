var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку контроллеров с представлениями
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Маршрут по умолчанию: Home/Index. Можно добавить маршрут для Crypto.
app.MapControllerRoute(
    name: "default",
        pattern: "{controller=Crypto}/{action=Index}/{id?}");

app.Run();
