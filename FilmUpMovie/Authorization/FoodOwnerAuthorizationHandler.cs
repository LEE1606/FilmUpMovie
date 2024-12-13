using FilmUpMovie.Models;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace FilmUpMovie.Authorization
{
    public class FoodOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Food>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public FoodOwnerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Food resource)
        {
            if (context.User == null || resource == null) return Task.CompletedTask;

            // If the requirement is Read, Update, or Delete, check the ownership
            if (requirement.Name != Constants.ReadOperationName &&
                  requirement.Name != Constants.UpdateOperationName &&
                  requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerID != null && resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}