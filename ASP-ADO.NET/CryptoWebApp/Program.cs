var builder = WebApplication.CreateBuilder(args);

// ��������� ��������� ������������ � ���������������
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ������� �� ���������: Home/Index. ����� �������� ������� ��� Crypto.
app.MapControllerRoute(
    name: "default",
        pattern: "{controller=Crypto}/{action=Index}/{id?}");

app.Run();
