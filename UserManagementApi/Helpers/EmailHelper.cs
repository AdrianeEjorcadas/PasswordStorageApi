using System.Net;
using System.Net.Mail;

public class EmailHelper
{
    private readonly IConfiguration _configuration;

    public EmailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmail(string toEmail, string subject, string body)
    {
        // Get SMTP configuration from appsettings.json
        var smtpConfig = _configuration.GetSection("Smtp");
        var smtpClient = new SmtpClient
        {
            Host = smtpConfig["Host"],
            Port = int.Parse(smtpConfig["Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpConfig["Username"], smtpConfig["Password"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpConfig["Username"], "Adriane Test"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
