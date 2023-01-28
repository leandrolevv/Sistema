using System.Text.Json;
using System.Text.Json.Serialization;
using Main;
using Main.DbContextSistema;
using Main.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var _connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<DbContextAccount>(options => options.UseSqlServer(_connectionString));
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<TokenService>();

Configuration.JwtToken = builder.Configuration.GetValue<string>("JwtToken");

var app = builder.Build();
app.MapControllers();

app.Run();