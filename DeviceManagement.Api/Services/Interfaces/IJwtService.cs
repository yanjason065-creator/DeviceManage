using DeviceManagement.Api.DTOs.Auth;
using DeviceManagement.Api.Models;

namespace DeviceManagement.Api.Services.Interfaces
{
    public interface IJwtService
    {
        LoginResponseDto GenerateToken(User user);
    }
}
