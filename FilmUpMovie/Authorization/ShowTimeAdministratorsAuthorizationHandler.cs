using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FilmUpMovie.Authorization
{
	public class ShowTimeAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ShowTime>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ShowTime resource)
		{
			if (context.User == null)
			{
				return Task.CompletedTask;
			}

			// Administrators can do anything.
			if (context.User.IsInRole(Constants.ShowTimeAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
