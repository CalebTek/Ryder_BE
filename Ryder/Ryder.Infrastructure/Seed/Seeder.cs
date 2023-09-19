﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ryder.Domain.Context;
using Ryder.Infrastructure.Policy;
using System.Security.Cryptography;

namespace Ryder.Infrastructure.Seed
{
    public static class Seeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            //Get db context
            var dbContext = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<ApplicationContext>();

            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await dbContext.Database.MigrateAsync();
            }

            if (dbContext.Users.Any())
            {
                return;
            }
            else
            {
                await dbContext.Database.EnsureCreatedAsync();

                var userManager = app.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var roleManager = app.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //Creating list of roles

                List<string> roles = new() { Policies.Rider, Policies.Customer };

                //Creating roles
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);


                AppUser user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "JohnDoe",
                    Email = "johndoe@example.com",
                    PhoneNumber = "00000000000",
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    Address = new Address
                    {
                        Id = Guid.NewGuid(),
                        City = "Warri",
                        State = "Delta",
                        PostCode = "+234",
                        Longitude = 3,
                        Latitude = 4,
                        Country = "Nigeria"
                    },
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30),
                };

                await userManager.CreateAsync(user, "Password@123");
                await userManager.AddToRoleAsync(user, roles[1]);


                //Saving everything into the database
                await dbContext.SaveChangesAsync();
            }
        }
    }
}