using DeviceManagement.Api.DTOs.Auth;

namespace DeviceManagement.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
