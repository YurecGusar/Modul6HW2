using Modul6Hw2.Data.Abstractions;
using Modul6Hw2.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6Hw2.Data.Repositories
{
    public class UserRepositori : IUserRepositori
    {
        private readonly ApplicationDbContext _ctx;
        public UserRepositori(
            ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<User> FindAsync(string userId)
        {
            var user = await _ctx.Users.FindAsync(userId);
            return user;
        }
    }
}
