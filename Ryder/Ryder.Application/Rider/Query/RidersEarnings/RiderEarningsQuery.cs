using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.Rider.Query.RidersEarnings
{
    public class RiderEarningsQuery : IRequest<IResult<RiderEarningsResponse>>
    {
        public Guid RiderId { get; set; }
    }

    public class RiderEarningQueryValidator : AbstractValidator<RiderEarningsQuery>
    {
        public RiderEarningQueryValidator()
        {
            RuleFor(x => x.RiderId).NotEmpty().NotNull().WithMessage("RiderId cannot be null or empty");
        }
    }
}
