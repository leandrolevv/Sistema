using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace Main.Services
{
    public class EmailService
    {
        public bool Send(string toEmail, string toName)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Configuration.Smtp.SmtpClient, Configuration.Smtp.Port);

                mail.From = new MailAddress(Configuration.FromEmail, Consts.SYSNAME);
                mail.To.Add(new MailAddress(toEmail, toName));
                
                WelcomeMessage(mail);

                SmtpServer.Port = Configuration.Smtp.Port;
                SmtpServer.Credentials = new NetworkCredential(Configuration.Smtp.Username, Configuration.Smtp.Password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void WelcomeMessage(MailMessage mail)
        {
            mail.Subject = $"Bem vindo ao {Consts.SYSNAME}";
            mail.Body = "Aproveite!";
        }
    }
}