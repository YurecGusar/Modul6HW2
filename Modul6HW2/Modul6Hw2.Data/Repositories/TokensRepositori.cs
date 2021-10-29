using Modul6Hw2.Data.Abstractions;
using Modul6Hw2.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6Hw2.Data.Repositories
{
    public class TokensRepositori : ITokensRepositori
    {
        private readonly ApplicationDbContext _ctx;
        public TokensRepositori(
            ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddAsync(RefreshToken token)
        {
            _ctx.Tokens.Add(token);
            await _ctx.SaveChangesAsync();
        }
    }
}
