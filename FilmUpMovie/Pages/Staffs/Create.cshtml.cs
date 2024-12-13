using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.Staffs
{
    public class CreateModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public CreateModel(FilmUpMovie.Data.FilmUpMovieContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Staff Staff { get; set; } = null!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add the new Staff
            _context.Staffs.Add(Staff);
            await _context.SaveChangesAsync();

            // Automatically create default Payment and LeaveApplication records
            var staffId = Staff.ID; // Retrieve the newly created Staff ID

            // Add a default Payment record
            var payment = new Payment
            {
                TotalWorkTime = 0, // Default value
                PayDate = DateTime.Today, // Default to today
                PayAmount = 0, // Default value
                StaffID = staffId
            };
            _context.Payments.Add(payment);

            // Add a default LeaveApplication record
            var leaveApplication = new LeaveApplication
            {
                LeaveAppTime = TimeSpan.Zero, // Default value
                LeaveAppDate = DateTime.Today, // Default to today
                LeaveAppReason = "Default Leave Reason", // Default reason
                StaffID = staffId
            };
            _context.LeaveApplications.Add(leaveApplication);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

