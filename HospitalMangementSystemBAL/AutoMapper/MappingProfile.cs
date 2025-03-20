using AutoMapper;
using HospitalManagementSystemDAL.Context;
using HospitalManagementSystemDAL.Models;
using HospitalManagementSystemShared.ViewModels;

namespace HospitalManagementSystemBAL.AutoMapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Appointment,AppointmentVM>().ReverseMap();
            Mapper.CreateMap<LoginModel, LoginViewModel>().ReverseMap();
            Mapper.CreateMap<RegisterModel, RegisterViewModel>().ReverseMap();
            Mapper.CreateMap<ApplicationUser, DoctorVM>().ReverseMap();
        }
    }
}
