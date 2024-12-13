using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.LeaveApplications
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
            ViewData["StaffID"] = new SelectList(_context.Staffs, "ID", "StaffEmail");
            return Page();
        }

        [BindProperty]
        public LeaveApplication LeaveApplication { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.LeaveApplications == null || LeaveApplication == null)
            {
                return Page();
            }

            // Assign the OwnerID to the current logged-in user's ID
            LeaveApplication.OwnerID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Set the initial status to "Submitted"
            LeaveApplication.Status = LeaveApplicationStatus.Submitted;

            _context.LeaveApplications.Add(LeaveApplication);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

