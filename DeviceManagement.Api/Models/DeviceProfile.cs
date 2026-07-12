using AutoMapper;
using DeviceManagement.Api.DTOs;

namespace DeviceManagement.Api.Models
{
    public class DeviceProfile: Profile
    {
        public DeviceProfile() {
            CreateMap<Device, DeviceDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.Employee.Name))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DeleteStatus,
                opt => opt.MapFrom(src => src.IsDeleted));

     
                

        }
    }
}
