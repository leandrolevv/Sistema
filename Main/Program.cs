using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Main;
using Main.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
Configuration.SetConfiguration(builder);

var app = builder.Build();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.Run();