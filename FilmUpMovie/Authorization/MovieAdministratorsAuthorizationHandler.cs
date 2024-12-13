using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FilmUpMovie.Authorization
{
	public class MovieAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Movie>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Movie resource)
		{
			if (context.User == null)
			{
				return Task.CompletedTask;
			}

			// Administrators can do anything.
			if (context.User.IsInRole(Constants.MovieAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
