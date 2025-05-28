using WebApplication1.Interfaces;
using WebApplication1.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

SeedData(app.Services);

app.Run();

static void SeedData(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    var category1 = new WebApplication1.Models.Category { Name = "Electronics" };
    var category2 = new WebApplication1.Models.Category { Name = "Books" };

    unitOfWork.Categories.AddAsync(category1).Wait();
    unitOfWork.Categories.AddAsync(category2).Wait();

    var product1 = new WebApplication1.Models.Product { Name = "Laptop", Price = 999.99m, CategoryId = 1 };
    var product2 = new WebApplication1.Models.Product { Name = "C# Programming", Price = 49.99m, CategoryId = 2 };

    unitOfWork.Products.AddAsync(product1).Wait();
    unitOfWork.Products.AddAsync(product2).Wait();
}