using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Smoower.Minified.Hosting;
using Smoower.Minified.SampleApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDb>(o => o.UseInMemoryDatabase("sample"));
builder.Services.single<Clock>();
builder.Services.scoped<IValidator<UserIn>, UserInValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Users.AddRange(
        new User { Name = "Ada Lovelace", Email = "ada@example.com" },
        new User { Name = "Alan Turing", Email = "alan@example.com" });
    db.SaveChanges();
}

app.MapControllers();
app.Run();
