using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
namespace FilmUpMovie.Pages.LeaveApplications
{
    public class EditModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public EditModel(FilmUpMovie.Data.FilmUpMovieContext context)
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
            LeaveApplication = leaveapplication;
            ViewData["StaffID"] = new SelectList(_context.Staffs, "ID", "StaffEmail");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LeaveApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveApplicationExists(LeaveApplication.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LeaveApplicationExists(int id)
        {
            return (_context.LeaveApplications?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
