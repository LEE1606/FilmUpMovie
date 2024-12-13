using FilmUpMovie.Models;
using FilmUpMovie.Authorization;
//using FilmUpMovie.Authorization.CinemaAuthorization;

using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FilmUpMovie.Data
{
	public static class DbInitializer
	{

        public static void Initialize(FilmUpMovieContext context, string adminID)
        {
            // Look for any movies or cinemas.
            if (context.Movie.Any() || context.Cinema.Any())
            {
                return; // DB has been seeded
            }

            var movies = new Movie[]
            {
        new Movie
        {
            Title = "Spiderman",
            ReleaseDate = DateTime.Parse("2017-09-01"),
            Price = 12,
            Genre = "Action",
            Rating = "PG13",
            Status = MovieStatus.Available,
            MovieOwnerId = adminID,
            PosterImage = GetImageBytes("wwwroot/images/spiderman.jpg") // assuming you store the file path
        },
        new Movie
        {
            Title = "Avengers: Endgame",
            ReleaseDate = DateTime.Parse("2019-04-26"),
            Price = 15,
            Genre = "Action",
            Rating = "PG13",
            Status = MovieStatus.Available,
            MovieOwnerId = adminID,
            PosterImage = GetImageBytes("wwwroot/images/avengers_endgame.jpg")
        },
        new Movie
        {
            Title = "The Lion King",
            ReleaseDate = DateTime.Parse("2019-07-19"),
            Price = 10,
            Genre = "Animation",
            Rating = "G",
            Status = MovieStatus.Available,
            MovieOwnerId = adminID,
            PosterImage = GetImageBytes("wwwroot/images/lion_king.jpg")
        },
        new Movie
        {
            Title = "Joker",
            ReleaseDate = DateTime.Parse("2019-10-04"),
            Price = 14,
            Genre = "Drama",
            Rating = "R",
            Status = MovieStatus.Available,
            MovieOwnerId = adminID,
            PosterImage = GetImageBytes("wwwroot/images/joker.jpg")
        },
        new Movie
        {
            Title = "Frozen II",
            ReleaseDate = DateTime.Parse("2019-11-22"),
            Price = 12,
            Genre = "Animation",
            Rating = "PG",
            Status = MovieStatus.Available,
            MovieOwnerId = adminID,
            PosterImage = GetImageBytes("wwwroot/images/frozen_ii.jpg")
        },
        new Movie
        {
            Title = "Toy Story 4",
            ReleaseDate = DateTime.Parse("2019-06-21"),
            Price = 11,
            Genre = "Animation",
            Rating = "G",
            Status = MovieStatus.Available,
            MovieOwnerId = adminID,
            PosterImage = GetImageBytes("wwwroot/images/toy_story_4.jpg")
        }
            };

            context.AddRange(movies);

            var cinemas = new Cinema[]
            {
        new Cinema
        {
            Name = "Cineplex FilmUp",
            Location = "Ipoh",
            NumOfHall = 3,
            Status = CinemaStatus.Approved,
            CinemaOwnerid = adminID,
            CinemaImage = GetImageBytes("wwwroot/images/aeon_cinema.jpg")
        },
        new Cinema
        {
            Name = "FilmUp Luxe Theaters",
            Location = "Kuala Lumpur",
            NumOfHall = 5,
            Status = CinemaStatus.Approved,
            CinemaOwnerid = adminID,
            CinemaImage = GetImageBytes("wwwroot/images/gsc_cinema.jpg")
        },
        new Cinema
        {
            Name = "Cinema City FilmUp",
            Location = "Penang",
            NumOfHall = 4,
            Status = CinemaStatus.Approved,
            CinemaOwnerid = adminID,
            CinemaImage = GetImageBytes("wwwroot/images/tgv_cinema.jpg")
        },
        //new Cinema
        //{
        //    Name = "MBO Cinemas",
        //    Location = "Johor Bahru",
        //    NumOfHall = 3,
        //    Status = CinemaStatus.Approved,
        //    CinemaOwnerid = adminID,
        //    CinemaImage = GetImageBytes("images/mbo_cinema.jpg")
        //},
        //new Cinema
        //{
        //    Name = "Cineplex",
        //    Location = "Melaka",
        //    NumOfHall = 2,
        //    Status = CinemaStatus.Approved,
        //    CinemaOwnerid = adminID,
        //    CinemaImage = GetImageBytes("images/cineplex_cinema.jpg")
        //},
        //new Cinema
        //{
        //    Name = "LFS Cinemas",
        //    Location = "Shah Alam",
        //    NumOfHall = 6,
        //    Status = CinemaStatus.Approved,
        //    CinemaOwnerid = adminID,
        //    CinemaImage = GetImageBytes("images/lfs_cinema.jpg")
        //}
            };

            context.AddRange(cinemas);

            var showtimes = new ShowTime[]
            {
        new ShowTime
        {
            Time = "12:00",
            Movie = movies[0],
            Status = ShowTimeStatus.Approved,
            TimeOwnerId = adminID
        },
        new ShowTime
        {
            Time = "15:00",
            Movie = movies[1],
            Status = ShowTimeStatus.Approved,
            TimeOwnerId = adminID
        },
        new ShowTime
        {
            Time = "18:00",
            Movie = movies[2],
            Status = ShowTimeStatus.Approved,
            TimeOwnerId = adminID
        },
        new ShowTime
        {
            Time = "21:00",
            Movie = movies[3],
            Status = ShowTimeStatus.Approved,
            TimeOwnerId = adminID
        },
        new ShowTime
        {
            Time = "10:00",
            Movie = movies[4],
            Status = ShowTimeStatus.Approved,
            TimeOwnerId = adminID
        },
        new ShowTime
        {
            Time = "13:30",
            Movie = movies[5],
            Status = ShowTimeStatus.Approved,
            TimeOwnerId = adminID
        }
            };

            context.AddRange(showtimes);
          

            var showrooms = new ShowRoom[]
            {
        new ShowRoom
        {
            RoomNum = "R1",
            TotalSeat = 100,
            Experience = "IMAX",
            Cinema = cinemas[0],
            Status = ShowRoomStatus.Approved,
            RoomOwnerId = adminID,
            ShowTimes = new List<ShowTime> { showtimes[0] }
        },
        new ShowRoom
        {
            RoomNum = "R2",
            TotalSeat = 120,
            Experience = "3D",
            Cinema = cinemas[1],
            Status = ShowRoomStatus.Approved,
            RoomOwnerId = adminID,
            ShowTimes = new List<ShowTime> { showtimes[1] }
        },
        new ShowRoom
        {
            RoomNum = "R3",
            TotalSeat = 150,
            Experience = "ScreenX",
            Cinema = cinemas[2],
            Status = ShowRoomStatus.Approved,
            RoomOwnerId = adminID,
            ShowTimes = new List<ShowTime> { showtimes[2] }
        },
        new ShowRoom
        {
            RoomNum = "R4",
            TotalSeat = 200,
            Experience = "4DX",
            Cinema = cinemas[0],
            Status = ShowRoomStatus.Approved,
            RoomOwnerId = adminID,
            ShowTimes = new List<ShowTime> { showtimes[3] }
        },
        new ShowRoom
        {
            RoomNum = "R5",
            TotalSeat = 250,
            Experience = "VIP",
            Cinema = cinemas[1],
            Status = ShowRoomStatus.Approved,
            RoomOwnerId = adminID,
            ShowTimes = new List<ShowTime> { showtimes[4] }
        },
        new ShowRoom
        {
            RoomNum = "R6",
            TotalSeat = 80,
            Experience = "Standard",
            Cinema = cinemas[2],
            Status = ShowRoomStatus.Approved,
            RoomOwnerId = adminID,
            ShowTimes = new List<ShowTime> { showtimes[5] }
        }
            };

            context.AddRange(showrooms);
            context.SaveChanges();


            var seats = new List<Seat>();
            foreach (var showtime in showtimes)
            {
                int rows = 5; // Number of rows of seats
                int seatsPerRow = 4; // Number of seats per row
                decimal basePrice = 10.00M; // Base price for seats
                for (int row = 1; row <= rows; row++)
                {
                    for (int seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
                    {
                        decimal price = basePrice + (row - 1) * 2; // Increment price per row
                        seats.Add(new Seat
                        {
                            SeatNumber = $"{row}-{seatNumber}", // Seat number format
                            IsAvailable = true, // Initially available
                            Price = price,
                            ShowTimeID = showtime.ShowTimeID,
                            Status = "Available"  // Initially available
                        });
                    }
                }
            }
            context.Seat.AddRange(seats);
            context.SaveChanges();


        }

        private static byte[] GetImageBytes(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                return File.ReadAllBytes(imagePath); // Reads the image as byte array
            }
            else
            {
                throw new FileNotFoundException($"The image file '{imagePath}' was not found.");
            }
        }
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new FilmUpMovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<FilmUpMovieContext>>()))
            {
                // Ensure Admin User
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@gmail.com");
                await EnsureRoles(serviceProvider, adminID, new[]
                {
            Constants.MovieAdministratorsRole,
            Constants.CinemaAdministratorsRole,
            Constants.ShowRoomAdministratorsRole,
            Constants.ShowTimeAdministratorsRole,
            Constants.FoodAdministratorsRole,
            Constants.BeverageAdministratorsRole,
            Constants.ComboAdministratorsRole,


        });

                // Ensure Manager User
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@gmail.com");
                await EnsureRoles(serviceProvider, managerID, new[]
                {
            Constants.MovieManagersRole,
            Constants.CinemaManagerRole,
            Constants.ShowRoomManagersRole,
            Constants.ShowTimeManagersRole,
            Constants.FoodManagerRole,
            Constants.BeverageManagerRole,
            Constants.ComboManagerRole

        });

                // Initialize database with admin ID
                Initialize(context, adminID);
            }
        }

       private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                              string testUserPw, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task EnsureRoles(IServiceProvider serviceProvider, string uid, string[] roles)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager is null");
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("User not found!");
            }

            foreach (var role in roles)
            {
                // Ensure the role exists
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Add the user to the role if not already in it
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}