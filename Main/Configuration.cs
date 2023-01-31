using System.Text;

namespace Main
{
    public static class Configuration
    {
        public static byte[] JwtToken { get; set; } = new byte[32];
        public static string AzureBlobConnectionString { get; set; } = String.Empty;
        public static string AzureBlobContainerImageUser { get; set; } = String.Empty;
        public static List<string> ProtectedRoles { get;} = new List<string> { Consts.RoleConstante.ADMIN, Consts.RoleConstante.USER };
        public static SmtpConfig Smtp { get; set; } = new ();
        public static string FromEmail { get; set; }

        public static void SetConfiguration(WebApplicationBuilder builder)
        {
            JwtToken = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtToken"));
            AzureBlobConnectionString = builder.Configuration.GetValue<string>("AzureBlobConnectionString");
            AzureBlobContainerImageUser = builder.Configuration.GetValue<string>("AzureBlobContainerImageUser");
            FromEmail = builder.Configuration.GetValue<string>("FromEmail");

            SmtpConfig smtp = new SmtpConfig();
            builder.Configuration.GetSection("Smtp").Bind(smtp);
            Smtp = smtp;


        }

        public class SmtpConfig
        {
            public int Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string SmtpClient { get; set; }
        }
    }
}
