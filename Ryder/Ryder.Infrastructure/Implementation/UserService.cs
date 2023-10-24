using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using System;
using System.Threading.Tasks;

namespace Ryder.Infrastructure.Implementation
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManger;

		public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManger)
		{
			_userManager = userManager;
			_signInManger = signInManger;
		}

		public async Task<AppUser> ValidateUserAsync(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user != null && await _userManager.CheckPasswordAsync(user, password))
			{
				return user;
			}

			return null;
		}

		public async Task<AppUser> RegisterUserAsync(string email, string password, string firstName, string lastName, string phoneNumber)
		{
			var user = new AppUser
			{
				UserName = email,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				PhoneNumber = phoneNumber,
				EmailConfirmed = true // You can adjust this based on your email confirmation process
			};

			var result = await _userManager.CreateAsync(user, password);

			if (result.Succeeded)
			{
				return user;
			}

			throw new Exception("User registration failed."); // Handle this exception as needed
		}

		public async Task SignOutAsync()
		{
			await _signInManger.SignOutAsync();
		}

		// Add other user-related methods here as needed
	}
}