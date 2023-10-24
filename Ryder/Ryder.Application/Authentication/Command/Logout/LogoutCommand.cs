using AspNetCoreHero.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Authentication.Command.Logout
{
	public class LogoutCommand : IRequest<IResult<string>>
	{
	}
}