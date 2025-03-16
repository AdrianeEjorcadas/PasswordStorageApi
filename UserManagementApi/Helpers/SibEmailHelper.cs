using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace UserManagementApi.Helpers
{
    public class SibEmailHelper
    {
        private readonly string _apiKey;

        public SibEmailHelper(IConfiguration configuration)
        {
            // Retrieve API key from configuration
            _apiKey = configuration["Sendinblue:ApiKey"];
            Configuration.Default.ApiKey.Add("api-key", _apiKey);
        }

        public async System.Threading.Tasks.Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var apiInstance = new TransactionalEmailsApi();

            var email = new SendSmtpEmail
            {
                To = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(toEmail) },
                Subject = subject,
                HtmlContent = body,
                Sender = new SendSmtpEmailSender("Adriane Ejorcadas", "adrianeejocadas97@gmail.com") // Update with your sender details
            };

            try
            {
                var result = await apiInstance.SendTransacEmailAsync(email);
                Console.WriteLine($"Email sent successfully! Message ID: {result.MessageId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
