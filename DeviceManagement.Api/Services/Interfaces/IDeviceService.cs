using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Models.Common;

namespace DeviceManagement.Api.Services.Interfaces
{
    public interface IDeviceService
    {
      PagedResults<DeviceDto> GetDevices(DeviceQuery query);

        Device GetById(long id);
        Device Create(Device device);

        DeviceDto AddDevice(CreateDeviceDto dto);

        DeviceDto UpdateDevice(long id, UpdateDeviceDto dto);

        bool DeleteDevice(long id);
        bool Update(Device device);
        bool Delete(long id);
        
    }
}
