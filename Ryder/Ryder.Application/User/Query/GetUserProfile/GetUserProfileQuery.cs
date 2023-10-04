using AspNetCoreHero.Results;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.User.Query.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<IResult<UserProfileResponse>>
    {
        public string UserId { get; set; }

        public GetUserProfileQuery(string id) => UserId = id;

    }


    public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
    {
        public GetUserProfileQueryValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }

}


