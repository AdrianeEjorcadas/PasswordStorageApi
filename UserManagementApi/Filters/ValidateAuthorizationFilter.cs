using Microsoft.AspNetCore.Mvc.Filters;

namespace UserManagementApi.Filters
{
    public class ValidateAuthorizationFilter : ActionFilterAttribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //if (!context.HttpContext.Request.Headers.TryGetValue("Auth-Role", out  var authorizationHeader)) {
            throw new NotImplementedException();
        }
    }
}
