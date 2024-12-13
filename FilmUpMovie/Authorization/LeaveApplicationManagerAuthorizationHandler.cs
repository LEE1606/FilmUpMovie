using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualBasic;

namespace FilmUpMovie.Authorization
{
    public class LeaveApplicationManagerAuthorizationHandler
     : AuthorizationHandler<OperationAuthorizationRequirement, LeaveApplication>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       LeaveApplication resource)
        {
            if (context.User == null || resource == null) return Task.CompletedTask;

            if (requirement.Name != Constants.ApproveOperationName &&
                requirement.Name != Constants.RejectOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole("Manager"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
