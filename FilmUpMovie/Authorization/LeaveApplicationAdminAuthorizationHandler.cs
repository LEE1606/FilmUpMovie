using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FilmUpMovie.Authorization
{
    public class LeaveApplicationAdminAuthorizationHandler
    : AuthorizationHandler<OperationAuthorizationRequirement, LeaveApplication>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       LeaveApplication resource)
        {
            if (context.User == null) return Task.CompletedTask;

            if (context.User.IsInRole("Administrator"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
