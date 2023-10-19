using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ryder.Application.Authentication.Command.Login
{
	public class LoginCommandHandler : IRequestHandler<LoginCommand, IResult<LoginResponse>>
	{
		private readonly IUserService _userService;
		private readonly ITokenGeneratorService _tokenService;
		private readonly IConfiguration _configuration;
		private readonly UserManager<AppUser> _userManager;
		private readonly ApplicationContext _context;

        public LoginCommandHandler(IUserService userService, ITokenGeneratorService tokenService,
            IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
            _userManager = userManager;
        }
		public LoginCommandHandler(IUserService userService, ITokenGeneratorService tokenService, IConfiguration configuration, UserManager<AppUser> userManager, ApplicationContext context)
		{
			_userService = userService;
			_tokenService = tokenService;
			_configuration = configuration;
			_userManager = userManager;
			_context = context;
		}

		public async Task<IResult<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			// Validate user credentials
			var user = await _userService.ValidateUserAsync(request.Email, request.Password);

            if (user == null)
            {
                return Result<LoginResponse>.Fail("Invalid username or password");
            }

            // Generate token
            var token = await _tokenService.GenerateTokenAsync(user);

            // Get the user's roles
            var userRoles = await _userManager.GetRolesAsync(user);

			var query = from rider in _context.Riders
						join
						appUser in _context.Users on
						rider.AppUserId equals appUser.Id
						where appUser.Id == user.Id
						select rider.Id;
			var riderId = query.FirstOrDefault();

			var response = new LoginResponse
			{
				Token = token,
				UserId = user.Id,
				UserName = user.UserName,
				FullName = $"{user.FirstName} {user.LastName}",
				UserRole = userRoles.FirstOrDefault(),
				RiderId = riderId
			
			};

            return Result<LoginResponse>.Success(response);
        }
    }
}

