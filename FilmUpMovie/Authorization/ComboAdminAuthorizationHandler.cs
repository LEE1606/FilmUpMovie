using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FilmUpMovie.Authorization
{
    public class ComboAdminAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Combo>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Combo resource)
        {
            if (context.User == null) return Task.CompletedTask;

            // Allow Admin to do anything with Combo
            if (context.User.IsInRole(Constants.ComboAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}