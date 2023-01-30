using Main.DbContextSistema;
using Main.Services.CloudFunctions;
using Main.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace Main.Extension
{
    public static class BuilderExtension
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var _connectionString = builder.Configuration.GetConnectionString("Default");
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
        }
    }
}
