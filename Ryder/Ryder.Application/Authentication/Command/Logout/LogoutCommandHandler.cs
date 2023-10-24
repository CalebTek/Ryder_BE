using AspNetCoreHero.Results;
using MediatR;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Authentication.Command.Logout
{
	public class LogoutCommandHandler : IRequestHandler<LogoutCommand, IResult<string>>
	{
		private readonly IUserService _userService;

		public LogoutCommandHandler(IUserService userService)
		{
			_userService = userService;
		}

		public async Task<IResult<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
		{
			await _userService.SignOutAsync();

			return Result<string>.Success("Logout Sucessful");
		}
	}
}