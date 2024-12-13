using FilmUpMovie.Authorization; // Add this to reference the Constants class
using FilmUpMovie.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FilmUpMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FilmUpMovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<FilmUpMovieContext>>()))
            {
                // Ensure the data is only seeded once
                if (!context.Staffs.Any())
                {
                    var staffList = new[]
                    {
                        new Staff
                        {
                            StaffName = "Tan Mei Ling",
                            StaffAge = 31,
                            StaffEmail = "tml@gmail.com",
                            StaffPhoneNumber = 0112314789,
                            StaffAddress = "No. 23, Jalan Cempaka 2, Taman Sri Ampang, 68000 Ampang, Selangor.",
                            StaffWorkDate = DateTime.Parse("1993-3-13")
                        },
                        new Staff
                        {
                            StaffName = "Lim Jian Hong",
                            StaffAge = 28,
                            StaffEmail = "ljh@gmail.com",
                            StaffPhoneNumber = 0123114921,
                            StaffAddress = "No. 8, Lorong Merbau 6, Taman Desa Jaya, 93250 Kuching, Sarawak",
                            StaffWorkDate = DateTime.Parse("1996-1-31")
                        },
                        // Add other staff entries...
                    };
                    context.Staffs.AddRange(staffList);
                    context.SaveChanges();

                    var staffArray = context.Staffs.ToArray();

                    // Assign LeaveApplications to 3 fixed staff
                    context.LeaveApplications.AddRange(
                        new LeaveApplication
                        {
                            LeaveAppTime = TimeSpan.FromHours(8),
                            LeaveAppDate = DateTime.Parse("2024-01-01"),
                            LeaveAppReason = "Medical Leave",
                            StaffID = staffArray[0].ID // Tan Mei Ling
                        },
                        new LeaveApplication
                        {
                            LeaveAppTime = TimeSpan.FromHours(4),
                            LeaveAppDate = DateTime.Parse("2024-02-14"),
                            LeaveAppReason = "Personal Matters",
                            StaffID = staffArray[1].ID // Lim Jian Hong
                        },
                        new LeaveApplication
                        {
                            LeaveAppTime = TimeSpan.FromHours(7.5),
                            LeaveAppDate = DateTime.Parse("2024-03-03"),
                            LeaveAppReason = "Family Emergency",
                            StaffID = staffArray[2].ID // Wong Jia Ming
                        }
                    );

                    context.SaveChanges();
                }
            }
        }

        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new FilmUpMovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<FilmUpMovieContext>>()))
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                testUserPw ??= "Default@123";

                // Add roles
                string[] roles = { Constants.AdminRole, Constants.ManagerRole };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Add admin and manager
                var adminEmail = "admin@gmail.com";
                var managerEmail = "manager@gmail.com";

                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin == null)
                {
                    admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                    await userManager.CreateAsync(admin, testUserPw);
                    await userManager.AddToRoleAsync(admin, Constants.AdminRole);
                }

                var manager = await userManager.FindByEmailAsync(managerEmail);
                if (manager == null)
                {
                    manager = new IdentityUser { UserName = managerEmail, Email = managerEmail, EmailConfirmed = true };
                    await userManager.CreateAsync(manager, testUserPw);
                    await userManager.AddToRoleAsync(manager, Constants.ManagerRole);
                }
            }
        }

        private static async Task<string> EnsureUser(UserManager<IdentityUser> userManager, string testUserPw, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser { UserName = userName, EmailConfirmed = true };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task EnsureRole(UserManager<IdentityUser> userManager, string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
