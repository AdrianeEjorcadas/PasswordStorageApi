using Newtonsoft.Json.Linq;

namespace UserManagementApi.Helpers
{
    public class ResetLinkHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ResetLinkHelper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GenerateResetLink(string token)
        {
            var prodUrl = _configuration["URL:BaseUrl"];
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var requestOrigin = GetRequestOrigin();

            if (string.Equals(prodUrl, requestOrigin, StringComparison.OrdinalIgnoreCase))
            {
                return $"{prodUrl}/reset-password?token={token}";
            }

            return $"{baseUrl}/api/reset-password?token={token}";
        }

        public string GenerateConfirmationLink(string token)
        {
            var prodUrl = _configuration["URL:BaseUrl"];
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var requestOrigin = GetRequestOrigin();

            if (string.Equals(prodUrl, requestOrigin, StringComparison.OrdinalIgnoreCase))
            {
                return $"{prodUrl}/confirm-email?token={token}";
            }
            return $"{baseUrl}/api/confirm-email?token={token}";
        }

        public string GenerateLoginLink()
        {
            var prodUrl = _configuration["URL:BaseUrl"];
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var requestOrigin = GetRequestOrigin();

            if (string.Equals(prodUrl, requestOrigin, StringComparison.OrdinalIgnoreCase))
            {
                return $"{prodUrl}/login";
            }
            return $"{baseUrl}/api/login";
        }

        public string GenerateForgotPasswordLink()
        {
            var prodUrl = _configuration["URL:BaseUrl"];
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var requestOrigin = GetRequestOrigin();

            if (string.Equals(prodUrl, requestOrigin, StringComparison.OrdinalIgnoreCase))
            {
                return $"{prodUrl}/forgot-password";
            }
            return $"{baseUrl}/api/forgot-password";

        }

        private string GetRequestOrigin()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var origin = request.Headers["Origin"].ToString();
            var referer = request.Headers["Referer"].ToString();

            return !string.IsNullOrEmpty(origin) ? origin :
                    (!string.IsNullOrEmpty(referer) ? referer : "Unknown origin");
        }
    }
}
