using Microsoft.AspNetCore.Mvc.Filters;

namespace UserManagementApi.Filters
{
    public class ValidateAuthorization : ActionFilterAttribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //if (!context.HttpContext.Request.Headers.TryGetValue("Auth-Role", out  var authorizationHeader)) {
            throw new NotImplementedException();
        }
    }
}
