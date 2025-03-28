using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagementApi.Models;

namespace UserManagementApi.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid) 
            {
                context.Result = new BadRequestObjectResult(new ErrorResponse
                {
                    StatusCode = 400,
                    ErrorMessage = "Invalid model state",
                    Details = context.ModelState.Values
                            .SelectMany(e => e.Errors)
                            .Select(e => e.ErrorMessage)
                            .FirstOrDefault()
                });
            }
        }
    }
}
