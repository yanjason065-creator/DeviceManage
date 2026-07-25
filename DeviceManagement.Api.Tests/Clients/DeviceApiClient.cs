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
        private const string deviceUrl = "/api/device";
        public DeviceApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetAllAsync(string? query = null) 
        {

            var url = deviceUrl;
            if(!string.IsNullOrEmpty(query))
            {
                url += query;
            }

            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> CreateAsync(CreateDeviceDto dto)
        {
            return await _client.PostAsJsonAsync(deviceUrl, dto);
        }

        public async Task<HttpResponseMessage> UpdateAsync(long id, UpdateDeviceDto dto)
        {
            return await _client.PutAsJsonAsync(
                $"{deviceUrl}/{id}",
                dto);
        }

        public async Task<HttpResponseMessage> DeleteAsync(long id)
        {
            return await _client.DeleteAsync($"{deviceUrl}/{id}");
        }

        public async Task<HttpResponseMessage> GetAsync(long id)
        {
            return await _client.GetAsync($"{deviceUrl}/{id}");
        }
    }
}
