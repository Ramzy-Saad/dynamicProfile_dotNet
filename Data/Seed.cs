using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;

namespace RunGroopWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // ✅ Correct way when using migrations
            context.Database.Migrate();

            // -------- Clubs --------
            if (!context.Clubs.Any())
            {
                context.Clubs.AddRange(
                    new Club
                    {
                        Title = "Running Club 1",
                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                        Description = "This is the description of the first club",
                        ClubCategory = ClubCategory.City,
                        Address = new Address
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    },
                    new Club
                    {
                        Title = "Running Club 2",
                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                        Description = "This is the description of the second club",
                        ClubCategory = ClubCategory.Endurance,
                        Address = new Address
                        {
                            Street = "456 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    },
                    new Club
                    {
                        Title = "Running Club 3",
                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                        Description = "This is the description of the third club",
                        ClubCategory = ClubCategory.Trail,
                        Address = new Address
                        {
                            Street = "789 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    }
                );

                context.SaveChanges();
            }

            // -------- Races --------
            if (!context.Races.Any())
            {
                context.Races.AddRange(
                    new Race
                    {
                        Title = "Running Race 1",
                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                        Description = "This is the description of the first race",
                        RaceCategory = RaceCategory.Marathon,
                        Address = new Address
                        {
                            Street = "123 Race St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    },
                    new Race
                    {
                        Title = "Running Race 2",
                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                        Description = "This is the description of the second race",
                        RaceCategory = RaceCategory.Ultra,
                        Address = new Address
                        {
                            Street = "456 Race St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    }
                );

                context.SaveChanges();
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            // Roles
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            // -------- Admin User --------
            string adminEmail = "admin@gmail.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Address = new Address
                    {
                        Street = "Admin St",
                        City = "Charlotte",
                        State = "NC"
                    }
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, UserRoles.Admin);
            }

            // -------- Normal User --------
            string userEmail = "user@gmail.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var user = new AppUser
                {
                    UserName = "app-user",
                    Email = userEmail,
                    EmailConfirmed = true,
                    Address = new Address
                    {
                        Street = "User St",
                        City = "Charlotte",
                        State = "NC"
                    }
                };

                var result = await userManager.CreateAsync(user, "User123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, UserRoles.User);
            }
        }
    }
}
