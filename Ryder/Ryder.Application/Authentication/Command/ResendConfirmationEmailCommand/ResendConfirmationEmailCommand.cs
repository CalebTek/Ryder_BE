using AspNetCoreHero.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Ryder.Application.Authentication.Command.ResendConfirmationEmailCommand
{
    public class ResendConfirmationEmailCommand : IRequest<IResult>
    {
        [Required] public string Email { get; set; }
    }
}