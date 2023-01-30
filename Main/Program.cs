using Main;
using Main.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
Configuration.SetConfiguration(builder);

var app = builder.Build();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();