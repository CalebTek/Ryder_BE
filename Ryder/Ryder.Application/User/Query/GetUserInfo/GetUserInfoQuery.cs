using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;

namespace Ryder.Application.User.Query.GetUserInfo
{
    public class GetUserInfoQuery : IRequest<IResult<GetUserInfoResponse>>
    {
        public string UserId { get; set; }

        public GetUserInfoQuery(string id)
        {
            UserId = id;
        }
    }

    public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
    {
        public GetUserInfoQueryValidator()
        {
            RuleFor(u=>u.UserId).NotNull().NotEmpty();
        }
    }

}
