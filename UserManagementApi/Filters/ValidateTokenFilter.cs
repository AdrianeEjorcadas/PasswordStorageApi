using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagementApi.CustomExceptions;
using UserManagementApi.Messages;
using UserManagementApi.Services;

namespace UserManagementApi.Filters
{
    public class ValidateTokenFilter : ActionFilterAttribute
    {
        private readonly IUserService _userService;
        public ValidateTokenFilter(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                // Check the header for auth token
                if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Result = new BadRequestObjectResult (new { ErrorMessage = "Authorization header is missing." });
                }
                // Extract the token
                var authTokenWithBearer = context.HttpContext.Request.Headers["Authorization"].ToString();
                var authToken = authTokenWithBearer.Replace("Bearer ", "").Trim();

                // Validate the token
                await _userService.ValidateTokenAsync(authToken);

                // Continue executing the action if the token is valid
                await next();
            }
            catch (RevokedTokenException ex)
            {
                context.Result = new StatusCodeResult(403); // status 403 will end the user session
            }
            catch (InvalidTokenException ex)
            {
                context.Result = new StatusCodeResult(401); // status 401 will be the trigger to generate new token to continue the session
            }
            catch (Exception ex)
            {
                context.Result = new StatusCodeResult(500);
            }
        }
    }
}
