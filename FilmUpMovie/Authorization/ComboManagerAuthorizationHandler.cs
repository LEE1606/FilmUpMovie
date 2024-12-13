using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FilmUpMovie.Authorization
{
    public class ComboManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Combo>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Combo resource)
        {
            if (context.User == null) return Task.CompletedTask;

            // Allow Managers to perform update, delete, and create
            if (context.User.IsInRole(Constants.ComboManagerRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}