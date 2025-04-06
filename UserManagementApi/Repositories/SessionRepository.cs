using UserManagementApi.Data;

namespace UserManagementApi.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task ClearHeaderAsync()
        {
            if (_httpContextAccessor != null)
            {
                _httpContextAccessor.HttpContext.Request.Headers.Remove("Authorization");
            }
            return Task.CompletedTask;
        }
    }
}
