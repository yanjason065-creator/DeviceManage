using AutoMapper;
using DeviceManagement.Api.Data;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.DTOs.Auth;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DeviceManagement.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        public AuthService(
            ILogger<AuthService> logger,
            AppDbContext context, 
            IMapper mapper,
            IJwtService jwtService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInformation("Login started. User: {Username}", request.Username);


            var user = await _context.Users.FirstOrDefaultAsync(d => d.Username == request.Username &&
             !d.IsDeleted);

            if (user == null)
            {
                _logger.LogWarning(
                    "User not found. User: {Username}",
                    request.Username);

                throw new UnauthorizedAccessException(
                    "Invalid username or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning(
                    "Invalid password. User: {Username}",
                    request.Username);

                throw new UnauthorizedAccessException(
                    "Invalid username or password.");
            }

            _logger.LogInformation(
                "Login successful. User: {Username}, Role: {Role}",
                user.Username,
                user.Role);

            //return new LoginResponseDto()
            //{
            //    Username = user.Username,
            //    Role = user.Role,
            //    Token = "",
            //    ExpiresAt = DateTime.UtcNow
            //};
            //
            return _jwtService.GenerateToken(user);
           
        }
    }
}
