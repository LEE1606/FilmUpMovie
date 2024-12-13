using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FilmUpMovie.Pages.LeaveApplications
{
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IConfiguration Configuration;
        private readonly IAuthorizationService AuthorizationService;
        private readonly UserManager<IdentityUser> UserManager;

        public IndexModel(
            FilmUpMovieContext context,
            IConfiguration configuration,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            Configuration = configuration;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }

        public string CurrentFilter { get; set; }
        public PaginatedList<LeaveApplication> LeaveApplications { get; set; }

        public async Task OnGetAsync(int? pageIndex, string searchString)
        {
            var leaveApplicationsIQ = from l in _context.LeaveApplications.Include(l => l.Staff)
                                      select l;

            // Get current user ID
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isManagerOrAdmin = User.IsInRole(Constants.ManagerRole) || User.IsInRole(Constants.AdminRole);

            // Filter by role
            if (!isManagerOrAdmin)
            {
                leaveApplicationsIQ = leaveApplicationsIQ.Where(l => l.Status == LeaveApplicationStatus.Approved || l.OwnerID == currentUserId);
            }

            // Apply search filter
            if (!String.IsNullOrEmpty(searchString))
            {
                leaveApplicationsIQ = leaveApplicationsIQ.Where(l => l.Staff.StaffName.Contains(searchString));
            }

            CurrentFilter = searchString;

            // Pagination
            var pageSize = Configuration.GetValue<int>("PageSize", 4);
            LeaveApplications = await PaginatedList<LeaveApplication>.CreateAsync(
                leaveApplicationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FirstOrDefaultAsync(l => l.ID == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            // Check if user has permission to approve
            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, leaveApplication, LeaveApplicationOperations.Approve);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            leaveApplication.Status = LeaveApplicationStatus.Approved;
            _context.LeaveApplications.Update(leaveApplication);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FirstOrDefaultAsync(l => l.ID == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            // Check if user has permission to reject
            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, leaveApplication, LeaveApplicationOperations.Reject);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            leaveApplication.Status = LeaveApplicationStatus.Rejected;
            _context.LeaveApplications.Update(leaveApplication);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
