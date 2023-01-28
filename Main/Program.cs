using Main.DbContextSistema;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var _connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<DbContextAccount>(options => options.UseSqlServer(_connectionString));
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

app.Run();