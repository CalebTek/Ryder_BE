using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;


namespace Ryder.Domain.SeedData
{
    public static class SeedData
    {
        public static void Initialize(UserManager<AppUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
            {
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);


                AppUser user = new AppUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Tobe",
                    LastName = "Nworgu",
                    ProfilePictureUrl = "https://example.com/profile.jpg",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    EmailConfirmed = false,
                    PhoneNumber = "08121183004",
                    PhoneNumberConfirmed = true,
                    Address = new Address
                    {
                        Id = Guid.NewGuid(),
                        City = "Warri",
                        State = "Delta",
                        PostCode = "+234",
                        Longitude = 3.ToString(),
                        Latitude = 4.ToString(),
                        Country = "Nigeria"
                    },
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30),
                };

                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "Password");
                IdentityResult result = userManager.CreateAsync(user, "Password").Result;

                if (result.Succeeded)
                {
                    // Add any additional properties to the user if needed
                    // For example, user.FirstName = "John";
                }
            }
        }
    }
}

