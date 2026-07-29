using Azure.Core;
using DeviceManagement.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Clients
{
    public class DeviceApiClient
    {
        private readonly HttpClient _client;
        private const string DeviceUrl = "/api/device";
        public DeviceApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetAllAsync(string? query = null) 
        {

            var url = BuildUrl(query);            

            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> CreateAsync(CreateDeviceDto dto)
        {
            return await _client.PostAsJsonAsync(DeviceUrl, dto);
        }

        public async Task<HttpResponseMessage> UpdateAsync(long id, UpdateDeviceDto dto)
        {
            return await _client.PutAsJsonAsync(
                $"{DeviceUrl}/{id}",
                dto);
        }

        public async Task<HttpResponseMessage> DeleteAsync(long id)
        {
            return await _client.DeleteAsync($"{DeviceUrl}/{id}");
        }

        public async Task<HttpResponseMessage> GetByIdAsync(long id)
        {
            return await _client.GetAsync($"{DeviceUrl}/{id}");
        }

        private static string BuildUrl(string? query)
        {
            if(string.IsNullOrEmpty(query))
            {
                return DeviceUrl;
            }

            return query.StartsWith("?")
                ? $"{DeviceUrl}{query}"
                :$"{DeviceUrl}?{query}";
        }
    }
}
