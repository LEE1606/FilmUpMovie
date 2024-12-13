using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace FilmUpMovie.Authorization
{
    public class BeverageOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Beverage>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public BeverageOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Beverage resource)
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