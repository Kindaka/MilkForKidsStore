using AutoMapper;
using MilkStore_BAL.ModelViews.AccountDTOs;
using MilkStore_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, UserAuthenticatingDtoResponse>().ReverseMap();
        }
    }
}
