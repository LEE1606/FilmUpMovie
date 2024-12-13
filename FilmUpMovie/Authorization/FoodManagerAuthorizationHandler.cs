using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FilmUpMovie.Authorization
{
    public class FoodManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Food>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Food resource)
        {
            if (context.User == null) return Task.CompletedTask;

            // Allow Managers to perform update, delete, and create
            if (context.User.IsInRole("FoodManagerRole"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}