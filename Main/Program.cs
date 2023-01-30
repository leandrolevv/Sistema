using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Main;
using Main.DbContextSistema;
using Main.Services;
using Main.Services.CloudFunctions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var _connectionString = builder.Configuration.GetConnectionString("Default");
Configuration.JwtToken = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtToken"));
Configuration.AzureBlobConnectionString = builder.Configuration.GetValue<string>("AzureBlobConnectionString");
Configuration.AzureBlobContainerImageUser = builder.Configuration.GetValue<string>("AzureBlobContainerImageUser");

builder.Services.AddDbContext<DbContextAccount>(options => options.UseSqlServer(_connectionString));
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<AzureFunctions>();
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