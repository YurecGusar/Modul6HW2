using AutoMapper;
using Modul6Hw2.Data.Abstractions;
using Modul6Hw2.Data.Entities;
using Modul6HW2.Services.Abstractions;
using Modul6HW2.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6HW2.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokensRepositori _tokenRepositori;
        private readonly IMapper _mapper;
        public TokenService(
            ITokensRepositori tokenRepositori,
            IMapper mapper)
        {
            _tokenRepositori = tokenRepositori;
            _mapper = mapper;
        }

        public async Task AddAsync(RefreshTokenModel model)
        {
            var entity = _mapper.Map<RefreshToken>(model);
            await _tokenRepositori.AddAsync(entity);
        }
    }
}
