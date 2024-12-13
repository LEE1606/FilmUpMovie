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

namespace FilmUpMovie.Pages.Payments
{
    public class EditModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public EditModel(FilmUpMovie.Data.FilmUpMovieContext context)
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

            var payment =  await _context.Payments.FirstOrDefaultAsync(m => m.ID == id);
            if (payment == null)
            {
                return NotFound();
            }
            Payment = payment;
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

            _context.Attach(Payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(Payment.ID))
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

        private bool PaymentExists(int id)
        {
          return (_context.Payments?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
