using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Modul6Hw2.Data.Abstractions;
using Modul6Hw2.Data.Entities;
using Modul6HW2.Services.Abstractions;
using Modul6HW2.Services.Configurations;
using Modul6HW2.Services.Models;

namespace Modul6HW2.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<UserModel> _userManager;
        private readonly ITokensRepositori _tokensRepositori;
        private readonly IMapper _mapper;

        public AuthService(
            ITokensRepositori tokensRepositori,
            IMapper mapper)
        {
            _tokensRepositori = tokensRepositori;
            _mapper = mapper;
        }

        public async Task<UserModel> ValidateUser(LoginCredentialsModel credentials)
        {

            var identityUser = await _userManager.FindByNameAsync(credentials.Username);
            var users = await _userManager.Users.ToListAsync();

            if (identityUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }

        public AccessAndRefreshTokenModel GenerateToken(IdentityUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email),
                    new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                }),

                Expires = DateTime.Now.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                }),

                Expires = DateTime.UtcNow.AddHours(jwtBearerTokenSettings.RefreshTokenExpiryTimeHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshTokenValue = tokenHandler.WriteToken(refreshToken);

            var model = _mapper.Map<RefreshToken>(new RefreshTokenModel
            {
                Value = refreshTokenValue,
                UserId = identityUser.Id
            });
            _tokensRepositori.AddAsync(model);

            return new AccessAndRefreshTokenModel
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshTokenValue
            };

        }
    }
}
