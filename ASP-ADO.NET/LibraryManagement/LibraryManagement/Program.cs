using Microsoft.EntityFrameworkCore;
using LibraryManagement.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ���������� SQLite ��� ��������
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite("Data Source=library.db"));

var app = builder.Build();

// �������������� �������� ���� ������ ��� �������
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    context.Database.EnsureCreated();
}

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
    pattern: "{controller=Authors}/{action=Index}/{id?}");

app.Run();