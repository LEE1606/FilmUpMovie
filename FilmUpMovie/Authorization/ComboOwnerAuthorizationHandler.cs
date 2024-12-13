using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace FilmUpMovie.Authorization
{
    public class ComboOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Combo>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ComboOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Combo resource)
        {
            if (context.User == null || resource == null) return Task.CompletedTask;

            var userId = _userManager.GetUserId(context.User);
            if (resource.OwnerID != null && resource.OwnerID == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}