using AspNetCoreHero.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ryder.Application.Authentication.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, IResult<LoginResponse>>
    {
        private readonly IUserService _userService;
        private readonly ITokenGeneratorService _tokenService;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IUserService userService, ITokenGeneratorService tokenService, IConfiguration configuration)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<IResult<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Validate user credentials
            var user = await _userService.ValidateUserAsync(request.Email, request.Password);

            if (user == null)
            {
                return Result<LoginResponse>.Fail("Invalid username or password");
            }

            // Generate token
            var token = await _tokenService.GenerateTokenAsync(user);

            // Retrieve JwtSettings from configuration
            var jwtSettings = new
            {
                SecretKey = _configuration["JwtSettings:SecretKey"],
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                TokenValidityInMinutes = int.Parse(_configuration["JwtSettings:TokenValidityInMinutes"]),
                RefreshTokenValidityInDays = int.Parse(_configuration["JwtSettings:RefreshTokenValidityInDays"])
            };

            var response = new LoginResponse
            {
                Token = token,

                // Add any other properties you want to return on successful login
            };

            return Result<LoginResponse>.Success(response);
        }
    }
}

