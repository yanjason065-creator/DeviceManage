using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Helpers
{
    public static class JsonHelper
    {
        public static async Task<T?> Deserialize<T>(
            HttpResponseMessage response)
        {
            var json = 
                await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}
