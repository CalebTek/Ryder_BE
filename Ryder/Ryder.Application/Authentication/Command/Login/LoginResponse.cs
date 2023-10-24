using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Authentication.Command.Login
{
	public class LoginResponse
	{
		public Guid UserId { get; set; } // Unique identifier for the user
		public string UserName { get; set; } // User's username or email
		public string FullName { get; set; } // User's full name
		public string Token { get; set; } // Authentication token (JWT) for subsequent requests
		public string UserRole { get; set; } // User's role
        public Guid RiderId { get; set; } // Rider Id

    }

}
