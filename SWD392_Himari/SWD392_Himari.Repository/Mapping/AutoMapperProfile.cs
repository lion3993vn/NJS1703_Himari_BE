using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SWD392_Himari.Repository.Entities;
using SWD392_Himari.Repository.ViewModel.User;

namespace SWD392_Himari.Repository.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //user
            CreateMap<Account, ViewUserModel>().ReverseMap();

        }
    }
}
