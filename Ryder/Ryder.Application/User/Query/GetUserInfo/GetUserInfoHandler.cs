using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ryder.Domain.Entities;
using Serilog;

namespace Ryder.Application.User.Query.GetUserInfo
{
    public class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, IResult<GetUserInfoResponse>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserInfoHandler(UserManager<AppUser> userManager) 
        {
            _userManager = userManager;
        }

        public async Task<IResult<GetUserInfoResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                var response = new GetUserInfoResponse()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                };

                return Result<GetUserInfoResponse>.Success(response);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"An error occurred: {ex.Message}");
                return Result<GetUserInfoResponse>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}
