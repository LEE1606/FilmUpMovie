using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FilmUpMovie.Authorization
{
	public class ShowRoomAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ShowRoom>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ShowRoom resource)
		{
			if (context.User == null)
			{
				return Task.CompletedTask;
			}

			// Administrators can do anything.
			if (context.User.IsInRole(Constants.ShowRoomAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
