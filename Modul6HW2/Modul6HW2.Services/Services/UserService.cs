using AutoMapper;
using Modul6Hw2.Data.Abstractions;
using Modul6Hw2.Data.Repositories;
using Modul6HW2.Services.Abstractions;
using Modul6HW2.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modul6HW2.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepositori _userRepo;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepositori userRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserModel> FindAsync(string userId)
        {
            var user = await _userRepo.FindAsync(userId);
            var model = _mapper.Map<UserModel>(user);
            return model;
        }
    }
}
