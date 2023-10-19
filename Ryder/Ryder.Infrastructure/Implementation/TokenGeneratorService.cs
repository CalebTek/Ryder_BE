using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ryder.Domain.Entities;
using Ryder.Infrastructure.Interface;
using static System.Int32;

namespace Ryder.Infrastructure.Implementation
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenGeneratorService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }


		public async Task<string> GenerateTokenAsync(AppUser user)
		{
			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			if (!string.IsNullOrWhiteSpace(user.FirstName))
				authClaims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));

			if (!string.IsNullOrWhiteSpace(user.LastName))
				authClaims.Add(new Claim(ClaimTypes.Surname, user.LastName));

			if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
				authClaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));

			// Get the roles of the logged-in user
			var roles = await _userManager.GetRolesAsync(user);

			// Add the user's roles as a comma-separated string claim
			authClaims.Add(new Claim("roles", string.Join(",", roles)));

			var signingKey =
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? string.Empty));
			TryParse(_configuration["JwtSettings:TokenValidityInMinutes"], out var tokenValidityInMinutes);

			// Specify JWTSecurityToken Parameters
			var token = new JwtSecurityToken
			(
				audience: _configuration["JwtSettings:Audience"],
				issuer: _configuration["JwtSettings:Issuer"],
				claims: authClaims,
				expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
				signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


		public async Task<string> GenerateRefreshTokenAsync(AppUser user)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            TryParse(_configuration["JwtSettings:RefreshTokenValidityInMinutes"],
                out var refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

            await _userManager.UpdateAsync(user);

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? string.Empty)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal =
                tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public JwtSecurityToken ReadToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
    }
}