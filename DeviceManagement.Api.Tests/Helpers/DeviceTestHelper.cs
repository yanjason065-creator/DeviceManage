using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Tests.TestData;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Tests.Assertions;

namespace DeviceManagement.Api.Tests.Helpers
{
    public class DeviceTestHelper
    {
        private readonly HttpClient _client;

        public DeviceTestHelper(HttpClient client)
        {
            _client = client;
        }

        public async Task<DeviceDto> CreateDeviceAsync(string? name = null)
        {
            var request = DeviceTestData.CreateValidRequest();

            if(!string.IsNullOrWhiteSpace(name))
            {
                request.Name = name;
            }

            var response = await _client.PostAsJsonAsync(
                "/api/device",
                request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<DeviceDto>>();

            ApiResponseAssertions.ShouldBeSuccessful(result);

            return result!.Data!;
        }

        public async Task<PagedResults<DeviceDto>> GetDevicesAsync(string query = "")
        {
            var response = await _client.GetAsync(
                $"/api/device{query}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            return result!.Data;
        }
    }
}
