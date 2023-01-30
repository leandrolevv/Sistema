using System.Text;

namespace Main
{
    public static class Configuration
    {
        public static byte[] JwtToken { get; set; } = new byte[32];
        public static string AzureBlobConnectionString { get; set; } = String.Empty;
        public static string AzureBlobContainerImageUser { get; set; } = String.Empty;
        public static List<string> ProtectedRoles { get;} = new List<string> { Constantes.RoleConstante.ADMIN, Constantes.RoleConstante.USER };

        public static void SetConfiguration(WebApplicationBuilder builder)
        {
            Configuration.JwtToken = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtToken"));
            Configuration.AzureBlobConnectionString = builder.Configuration.GetValue<string>("AzureBlobConnectionString");
            Configuration.AzureBlobContainerImageUser = builder.Configuration.GetValue<string>("AzureBlobContainerImageUser");
        }
    }
}
