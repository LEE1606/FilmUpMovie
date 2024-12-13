using FilmUpMovie.Models;  // Ensure this is included for the Cinema model
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using FilmUpMovie.Authorization;  // Ensure this is included for the Constants class

namespace FilmUpMovie.Authorization
{
    public class CinemaAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Cinema>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Cinema resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.CinemaAdministratorsRole))  // Correctly referencing the constant
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
