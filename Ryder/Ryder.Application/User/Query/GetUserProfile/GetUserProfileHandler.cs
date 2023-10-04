using AspNetCoreHero.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ryder.Domain.Context;
using Ryder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.User.Query.GetUserProfile
{
    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, IResult<UserProfileResponse>>
    {
        //prop
        private readonly UserManager<AppUser> _userManager;

        //ctor
        public GetUserProfileHandler(UserManager<AppUser> userManager) => _userManager = userManager;
         

        public async Task<IResult<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userManager.FindByIdAsync(request.UserId);

                var response = new UserProfileResponse
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    PhoneNumber = result.PhoneNumber,
                    Email = result.Email
                };

                return Result<UserProfileResponse>.Success(response);
                
            }
            catch (Exception)
            {
                return Result<UserProfileResponse>.Fail("Ooops Something Went Wrong");
            }
        }

    }
}
