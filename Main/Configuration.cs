using System.Text;

namespace Main
{
    public static class Configuration
    {
        public static byte[] JwtToken { get; set; }
        public static string AzureBlobConnectionString { get; set; }
        public static string AzureBlobContainerImageUser { get; set; }

        public static void SetConfiguration(WebApplicationBuilder builder)
        {
            Configuration.JwtToken = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtToken"));
            Configuration.AzureBlobConnectionString = builder.Configuration.GetValue<string>("AzureBlobConnectionString");
            Configuration.AzureBlobContainerImageUser = builder.Configuration.GetValue<string>("AzureBlobContainerImageUser");
        }
    }
}
