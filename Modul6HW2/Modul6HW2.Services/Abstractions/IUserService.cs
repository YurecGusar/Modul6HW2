using Modul6HW2.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6HW2.Services.Abstractions
{
    public interface IUserService
    {
        public Task<UserModel> FindAsync(string userId);
    }
}
