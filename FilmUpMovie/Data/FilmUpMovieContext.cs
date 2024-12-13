using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Models;

namespace FilmUpMovie.Data
{
    public class FilmUpMovieContext : IdentityDbContext
    {
        public FilmUpMovieContext (DbContextOptions<FilmUpMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Seat> Seat { get; set; }  
        public DbSet<Movie> Movie { get; set; }
		public DbSet<Cinema> Cinema { get; set; }

        public DbSet<ShowRoom> ShowRoom { get; set; }

        public DbSet<ShowTime> ShowTime { get; set; }
        public DbSet<Food> Foods { get; set; } // Add DbSet for Food
        public DbSet<Beverage> Beverages { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Blog> Blogs { get; set; } = default!; // Rename to "Blogs" to follow conventions
        public DbSet<Comment> Comments { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().ToTable("Movie");
			modelBuilder.Entity<Cinema>().ToTable("Cinema");
            modelBuilder.Entity<ShowTime>().ToTable("ShowTime");
			modelBuilder.Entity<ShowRoom>().ToTable("ShowRoom");
            modelBuilder.Entity<Cart>().ToTable("Cart");
            modelBuilder.Entity<CartItem>().ToTable("CartItem");

        }

    }
}
