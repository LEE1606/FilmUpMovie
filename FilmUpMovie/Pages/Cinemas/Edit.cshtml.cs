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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FilmUpMovie.Authorization;
using FilmUpMovie.ViewModels;

namespace FilmUpMovie.Pages.Cinemas
{
    public class EditModel : DI_BasePageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public EditModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        [BindProperty]
        public Cinema Cinemas { get; set; } = default!;
		//public IFormFile? CinemaImageFile { get; set; }

		[BindProperty]
		public CinemaViewModels CinemaViewModels { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Cinema == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinema.FirstOrDefaultAsync(m => m.CinemaID == id);
            if (cinema == null)
            {
                return NotFound();
            }

            // Map Food entity to FoodViewModel
            CinemaViewModels = new CinemaViewModels
            {
                Name = cinema.Name,
                Location = cinema.Location,
                NumOfHall = cinema.NumOfHall,
            };

            //Cinemas = cinema;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {

			if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var cinema = await _context.Cinema.FirstOrDefaultAsync(m => m.CinemaID == id);


            if (cinema == null)
            {
                return NotFound();
            }


            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, cinema,
                                                     Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }


            // Update Cinema entity with CinemaViewModel values
            cinema.Name = CinemaViewModels.Name;
            cinema.Location = CinemaViewModels.Location;
            cinema.NumOfHall = CinemaViewModels.NumOfHall;
            
          
            //Cinemas.CinemaOwnerid = cinema.CinemaOwnerid;

            if (CinemaViewModels.CinemaImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(CinemaViewModels.CinemaImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("PosterImageFile", "Only image files (jpg, jpeg, png) are allowed.");
                    return Page();
                }

                // Check file size (limit to 10MB)
                if (CinemaViewModels.CinemaImageFile.Length > 10 * 1024 * 1024)  // 10MB size limit
                {
                    ModelState.AddModelError("PosterImageFile", "The file size exceeds the 10MB limit.");
                    return Page();
                }

                // Convert the uploaded file to a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await CinemaViewModels.CinemaImageFile.CopyToAsync(memoryStream);
                    cinema.CinemaImage = memoryStream.ToArray(); // Store the byte array in PosterImage
                }
            }

            

            if (cinema.Status == CinemaStatus.Approved)
            {
                // If the contact is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
										cinema,
                                        Operations.Approve);

                if (!canApprove.Succeeded)
                {
					cinema.Status = CinemaStatus.Submitted;
                }
            }

            _context.Attach(cinema).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(cinema.CinemaID))
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

        private bool CinemaExists(int id)
        {
            return _context.Cinema.Any(e => e.CinemaID == id);
        }
    }
}
