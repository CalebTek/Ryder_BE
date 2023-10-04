using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.User.Command.EditUserProfile
{
    public class ProfileModel
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserPhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
