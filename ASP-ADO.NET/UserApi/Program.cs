using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ��������� Entity Framework � InMemory ����� ������
builder.Services.AddDbContext<UserDb>(opt => opt.UseInMemoryDatabase("UserList"));

var app = builder.Build();

// GET: �������� ���� �������������
app.MapGet("/users", async (UserDb db) =>
    await db.Users.ToListAsync());

// GET: �������� ������������ �� ID
app.MapGet("/users/{id}", async (int id, UserDb db) =>
    await db.Users.FindAsync(id)
        is User user
            ? Results.Ok(user)
            : Results.NotFound());

// POST: ������� ������ ������������
app.MapPost("/users", async (User user, UserDb db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
});

// PUT: �������� ������ ������������
app.MapPut("/users/{id}", async (int id, User inputUser, UserDb db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    user.Name = inputUser.Name;
    user.Email = inputUser.Email;
    user.Age = inputUser.Age;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE: ������� ������������
app.MapDelete("/users/{id}", async (int id, UserDb db) =>
{
    if (await db.Users.FindAsync(id) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();

// ������ ������������
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
}

// �������� ���� ������
class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
}