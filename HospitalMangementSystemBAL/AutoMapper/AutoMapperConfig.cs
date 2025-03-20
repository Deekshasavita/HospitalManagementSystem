using AutoMapper;
using HospitalManagementSystemBAL.AutoMapper;

namespace HospitalManagementSystemBAL
{
    internal class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); 
            });
        }
    }
}
