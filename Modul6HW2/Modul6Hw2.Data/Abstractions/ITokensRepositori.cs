using Modul6Hw2.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6Hw2.Data.Abstractions
{
    public interface ITokensRepositori
    {
        public Task AddAsync(RefreshToken token);
    }
}
