using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Main;
using Main.DbContextSistema;
using Main.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var _connectionString = builder.Configuration.GetConnectionString("Default");
Configuration.JwtToken = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtToken"));

builder.Services.AddDbContext<DbContextAccount>(options => options.UseSqlServer(_connectionString));
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<TokenService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Configuration.JwtToken)
    };
});

var app = builder.Build();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();