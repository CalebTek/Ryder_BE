using AspNetCoreHero.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Application.User.Command.EditUserProfile
{
    public class EditUserProfileComand : IRequest<IResult>
    {
        //props
        public string UserId { get; set; }
        public ProfileModel ProfileModel { get; }

        //ctor
        public EditUserProfileComand(string userId, ProfileModel profileModel)
        {
            UserId = userId; 
            ProfileModel = profileModel;
        }
    }


    public class EditUserProfileValidation : AbstractValidator<EditUserProfileComand>
    {
        public EditUserProfileValidation()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.ProfileModel.UserPhoneNumber).NotNull().NotEmpty();
            RuleFor(x => x.ProfileModel.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.ProfileModel.LastName).NotNull().NotEmpty();
            RuleFor(x => x.ProfileModel.Email).NotNull().NotEmpty().EmailAddress();
        }
    }
}
