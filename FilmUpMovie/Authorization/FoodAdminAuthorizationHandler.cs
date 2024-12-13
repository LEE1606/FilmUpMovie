using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FilmUpMovie.Authorization
{
    public class FoodAdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Food>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Food resource)
        {
            if (context.User == null) return Task.CompletedTask;

            // Allow Admin to do anything with Food
            if (context.User.IsInRole(Constants.FoodAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}