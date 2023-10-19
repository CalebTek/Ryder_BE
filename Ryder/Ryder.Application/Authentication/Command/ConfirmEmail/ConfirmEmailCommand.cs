using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Authentication.Command.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<IResult>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(request => request.Token)
                .NotEmpty()
                .NotNull();
        }
    }
}