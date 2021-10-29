using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modul6HW2.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6HW2.Services.Abstractions
{
    public interface IAuthService
    {
        public Task<UserModel> ValidateUser(LoginCredentialsModel credentials);
        public AccessAndRefreshTokenModel GenerateToken(IdentityUser identityUser);
    }
}
