using Microsoft.EntityFrameworkCore;
using SkiStore.Data;
using SkiStore.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:3000");
});

app.MapControllers();

DbInitializer.InitDb(app);

app.Run();
