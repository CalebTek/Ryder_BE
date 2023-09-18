﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.User.Query.Login
{
    public class LoginResponse
    {
        public string UserId { get; set; } // Unique identifier for the user
        public string UserName { get; set; } // User's username or email
        public string FullName { get; set; } // User's full name
        public string Token { get; set; } // Authentication token (JWT) for subsequent requests
        // Additional properties as needed (e.g., UserRole, UserEmail, etc.)
    }
}
