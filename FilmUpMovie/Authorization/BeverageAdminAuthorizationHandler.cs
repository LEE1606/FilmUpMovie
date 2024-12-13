using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FilmUpMovie.Authorization
{
    public class BeverageAdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Beverage>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Beverage resource)
        {
            if (context.User == null) return Task.CompletedTask;

            // Allow Admin to do anything with Beverage
            if (context.User.IsInRole(Constants.BeverageAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}