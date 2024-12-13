using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.Staffs
{
    public class DetailsModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public DetailsModel(FilmUpMovie.Data.FilmUpMovieContext context)
        {
            _context = context;
        }

        public Staff Staff { get; set; } = null!;
        public List<Payment> Payments { get; set; } = new();
        public List<LeaveApplication> LeaveApplications { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include related Payments and LeaveApplications
            Staff = await _context.Staffs.FirstOrDefaultAsync(s => s.ID == id);
            Payments = await _context.Payments.Where(p => p.StaffID == id).ToListAsync();
            LeaveApplications = await _context.LeaveApplications.Where(la => la.StaffID == id).ToListAsync();

            if (Staff == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}