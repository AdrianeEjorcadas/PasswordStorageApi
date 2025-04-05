using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagementApi.Services;

namespace UserManagementApi.Filters
{
    public class ValidateAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IUserService _userService;
        public ValidateAuthorizationFilter(IUserService userService)
        {
            _userService = userService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {

            }
            catch (Exception ex) 
            {
                context.Result = new StatusCodeResult(500);
            }
        }
    }
}
