using Ryder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Infrastructure.Interface
{
    public interface IUserService
    {
        Task<AppUser> ValidateUserAsync(string email, string password);
        Task<AppUser> RegisterUserAsync(string email, string password, string firstName, string lastName, string phoneNumber);

        Task SignOutAsync();
		// Add other user-related methods here as needed
	}
}
