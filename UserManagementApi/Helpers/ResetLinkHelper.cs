namespace UserManagementApi.Helpers
{
    public class ResetLinkHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResetLinkHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateResetLink(string token)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/api/reset-password?token={token}";
        }
    }
}
