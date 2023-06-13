using AutoMapper;
using MedicaBE.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicaBE.Entities.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginVM, Retailer>();
            //.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            //.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<RegistrationVM, Retailer>();
           //.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
           //.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
           //.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
           //.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
           //.ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword));
        }
    }
}
