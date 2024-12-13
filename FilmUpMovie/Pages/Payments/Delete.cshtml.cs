using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.Payments
{
    public class DeleteModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public DeleteModel(FilmUpMovie.Data.FilmUpMovieContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Payment Payment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FirstOrDefaultAsync(m => m.ID == id);

            if (payment == null)
            {
                return NotFound();
            }
            else 
            {
                Payment = payment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }
            var payment = await _context.Payments.FindAsync(id);

            if (payment != null)
            {
                Payment = payment;
                _context.Payments.Remove(Payment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
