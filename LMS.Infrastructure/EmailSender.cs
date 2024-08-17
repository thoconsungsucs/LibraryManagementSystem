using LMS.Domain.IService;
using LMS.Domain.Ultilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace LMS.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpName;
        private readonly string _smtpEmail;
        private readonly string _smtpPassword;


        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpHost = _configuration["MailSettings:Host"];
            _smtpPort = int.Parse(_configuration["MailSettings:Port"]);
            _smtpName = _configuration["MailSettings:Name"];
            _smtpEmail = _configuration["MailSettings:Email"];
            _smtpPassword = _configuration["MailSettings:Password"];
        }
        public async Task Send(MailInformation mailInformation)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpName, _smtpEmail));
            message.To.Add(new MailboxAddress(mailInformation.Name, mailInformation.Email));
            message.Subject = mailInformation.Subject;

            // Create the body of the email
            message.Body = new TextPart("plain")
            {
                Text = mailInformation.Content
            };

            // Configure the SMTP client and send the email
            using (var client = new SmtpClient())
            {
                try
                {
                    // Connect to the SMTP server
                    client.Connect(_smtpHost, 587, SecureSocketOptions.StartTls);

                    // Authenticate with the server
                    client.Authenticate(_smtpEmail, _smtpPassword);

                    // Send the email
                    await client.SendAsync(message);

                    // Disconnect from the server
                    client.Disconnect(true);

                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }

    }
}
