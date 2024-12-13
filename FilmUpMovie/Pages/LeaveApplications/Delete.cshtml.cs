using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.LeaveApplications
{
    public class DeleteModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public DeleteModel(FilmUpMovie.Data.FilmUpMovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LeaveApplication LeaveApplication { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.LeaveApplications == null)
            {
                return NotFound();
            }

            var leaveapplication = await _context.LeaveApplications.FirstOrDefaultAsync(m => m.ID == id);

            if (leaveapplication == null)
            {
                return NotFound();
            }
            else
            {
                LeaveApplication = leaveapplication;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.LeaveApplications == null)
            {
                return NotFound();
            }
            var leaveapplication = await _context.LeaveApplications.FindAsync(id);

            if (leaveapplication != null)
            {
                LeaveApplication = leaveapplication;
                _context.LeaveApplications.Remove(LeaveApplication);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
