using DeviceManagement.Api.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Clients
{
    public class AuthApiClient
    {
        private HttpClient _client { get; }
        private const string AuthUrl = "/api/auth";
        public AuthApiClient(HttpClient client) 
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _client.PostAsJsonAsync(
                $"{AuthUrl}/login",
                loginRequestDto);
        }
    }
}
