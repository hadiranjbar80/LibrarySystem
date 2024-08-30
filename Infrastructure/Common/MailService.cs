using Application.Interfaces;
using Infrastructure.Common.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Common
{
    public class MailService : IMailService
    {
        private readonly EmailSetting _emailSetting;

        public MailService(IOptions<EmailSetting> emailSetting)
        {
            _emailSetting = emailSetting.Value;
        }
        public async Task SendEmail(string email, string subject, string message)
        {
            try
            {
                var cridentials = new NetworkCredential(_emailSetting.Sender, _emailSetting.Password);

                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSetting.Sender, _emailSetting.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                mail.To.Add(new MailAddress(email));

                var client = new SmtpClient
                {
                    Port = _emailSetting.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host =_emailSetting.MailServer,
                    EnableSsl = true,
                    Credentials = cridentials
                };

                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
