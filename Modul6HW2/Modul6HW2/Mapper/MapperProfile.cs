using AutoMapper;
using Modul6Hw2.Data.Entities;
using Modul6HW2.Models.PostModels;
using Modul6HW2.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modul6HW2.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //PL
            CreateMap<UserDetailsPostModel, User>();
            CreateMap<LoginCredentialsPostModel, LoginCredentialsModel>();
            //BL
            CreateMap<RefreshTokenModel, RefreshToken>();
            CreateMap<User, UserModel>();
        }
    }
}
