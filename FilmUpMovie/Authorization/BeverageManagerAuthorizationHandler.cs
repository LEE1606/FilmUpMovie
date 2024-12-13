using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FilmUpMovie.Authorization
{
    public class BeverageManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Beverage>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Beverage resource)
        {
            if (context.User == null) return Task.CompletedTask;

            // Allow Managers to perform update, delete, and create
            if (context.User.IsInRole(Constants.BeverageManagerRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}