using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Identity;

namespace FilmUpMovie.Authorization
{
	public class ShowTimeIsOwnerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ShowTime>
	{
		UserManager<IdentityUser> _userManager;

		public ShowTimeIsOwnerAuthorizationHandler(UserManager<IdentityUser>
			userManager)
		{
			_userManager = userManager;
		}
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ShowTime resource)
		{
			if (context.User == null || resource == null)
			{
				return Task.CompletedTask;
			}

			// If not asking for CRUD permission, return.

			if (requirement.Name != Constants.CreateOperationName &&
				requirement.Name != Constants.ReadOperationName &&
				requirement.Name != Constants.UpdateOperationName &&
				requirement.Name != Constants.DeleteOperationName)
			{
				return Task.CompletedTask;
			}

			if (resource.TimeOwnerId == _userManager.GetUserId(context.User))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}

	}
}
