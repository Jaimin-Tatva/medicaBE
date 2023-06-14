using AutoMapper;
using MedicaBE.Entities.Models;
using MedicaBE.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
             CreateMap<UserRegisterViewModel, User>();

             CreateMap<User, UserInformationViewModel>();
        } 

    }
}
