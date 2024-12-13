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
    public class DetailsModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public DetailsModel(FilmUpMovie.Data.FilmUpMovieContext context)
        {
            _context = context;
        }

        public LeaveApplication LeaveApplication { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include Staff in the query
            LeaveApplication = await _context.LeaveApplications
                .Include(la => la.Staff)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (LeaveApplication == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
