using DeviceManagement.Api.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace DeviceManagement.Api.Tests.Infrastructure
{
    public class AuthenticationHelper
    {
        public static async Task<string> GetAdminTokenAsync(
            HttpClient client)
        {
            var request = new LoginRequestDto
            {
                Username = "admin",
                Password = "123456"
            };

            var response =
                await client.PostAsJsonAsync(
                    "/api/auth/login",
                    request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<LoginResponseDto>();

            return result!.Token;
        }

        public static async Task<string> GetUserTokenAsync(HttpClient client)
        {
            var request = new LoginRequestDto
            {
                Username = "JasonYan",
                Password = "123456"
            };

            var response = await client.PostAsJsonAsync(
                "/api/auth/login",
                request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<LoginResponseDto>();

            return result!.Token;
        }
    }
}
