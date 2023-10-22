using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Payment.Query
{
    public class VerifyPaymentQuery : IRequest<IResult<VerifyPaymentResponse>>
    {
        public string PaymentReference { get; set; }
    }

    public class VerifyPaymentQueryValidator : AbstractValidator<VerifyPaymentQuery>
    {
        public VerifyPaymentQueryValidator()
        {
            RuleFor(x => x.PaymentReference).NotNull().NotEmpty();
        }
    }
}
