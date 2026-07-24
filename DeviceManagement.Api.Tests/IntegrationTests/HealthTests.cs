using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using DeviceManagement.Api.Tests.Infrastructure;

namespace DeviceManagement.Api.Tests.Intergration
{
    public class HealthTests:
        IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public HealthTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Api_Should_Start_Successfully()
        {
            var response = await _client.GetAsync("/");

            response.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.OK);
        }
    }
}
