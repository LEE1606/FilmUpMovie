using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FilmUpMovie.Authorization
{
	public class CinemaManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Cinema>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Cinema resource)
		{
			if (context.User == null || resource == null)
			{
				return Task.CompletedTask;
			}

			// If not asking for approval/reject, return.
			if (requirement.Name != Constants.ApproveOperationName &&
				requirement.Name != Constants.RejectOperationName)
			{
				return Task.CompletedTask;
			}

			// Managers can approve or reject.
			if (context.User.IsInRole(Constants.CinemaManagerRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
