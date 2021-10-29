using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Modul6Hw2.Data.Entities;
using Modul6HW2.Configuration;
using Modul6HW2.Models;
using Modul6HW2.Models.PostModels;
using Modul6HW2.Services.Abstractions;
using Modul6HW2.Services.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Modul6HW2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService,
            IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
            _authService = authService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDetailsPostModel userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var identityUser = _mapper.Map<User>(userDetails);
            var result = await _userManager.CreateAsync(identityUser, userDetails.Password);

            if (!result.Succeeded)
            {
                RegistrationFailed(result);
            }

            return Ok(new { Message = "User Reigstration Successful" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsPostModel credentials)
        {
            UserModel identityUser;
            var model = _mapper.Map<LoginCredentialsModel>(credentials);
            if (!ModelState.IsValid
                || model == null
                || (identityUser = await _authService.ValidateUser(model)) == null)
            {
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }

            var token = _authService.GenerateToken(identityUser);
            return Ok(
                new
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    Username = "FIRST_NAME",
                    Roles = new[] { "ROLE_ADMIN", "ROLE_MODERATOR" },
                    Id = Guid.NewGuid().ToString()
                });
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Well, What do you want to do here ?
            // Wait for token to get expired OR 
            // Maintain token cache and invalidate the tokens after logout method is called
            return Ok(new { Token = "", Message = "Logged Out" });
        }

        [HttpPost]
        [Route("refreshtoken")]
        public async Task<AccessAndRefreshTokenModel> RefreshToken(string refreshToken)
        {
            SecurityToken validatedToken = null;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validationParameters, out validatedToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userService.FindAsync(userId);//_ctx.Users.Find(userId);

            var token = _authService.GenerateToken(user);

            return token;
        }

        private BadRequestObjectResult RegistrationFailed(IdentityResult result)
        {
            var dictionary = new ModelStateDictionary();
            foreach (IdentityError error in result.Errors)
            {
                dictionary.AddModelError(error.Code, error.Description);
            }

            return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
        }
    }
}
