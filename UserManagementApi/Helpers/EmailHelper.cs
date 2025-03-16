using System.Net;
using System.Net.Mail;

public class EmailHelper
{
    private readonly IConfiguration _configuration;

    public EmailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string to, string subject, string body)
    {
        // Get SMTP configuration from appsettings.json
        var smtpConfig = _configuration.GetSection("Smtp");
        var smtpClient = new SmtpClient
        {
            Host = smtpConfig["Host"],
            Port = int.Parse(smtpConfig["Port"]),
            EnableSsl = bool.Parse(smtpConfig["EnableSsl"]),
            Credentials = new NetworkCredential(smtpConfig["Username"], smtpConfig["Password"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpConfig["Username"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true // Set to true for HTML emails
        };

        mailMessage.To.Add(to);

        // Send the email
        smtpClient.Send(mailMessage);
    }
}
