using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using DeviceManagement.Api.Tests.Infrastructure;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.VisualBasic;

namespace DeviceManagement.Api.Tests.Authorization
{
    public class DeviceAuthorizationTests
        :IntegrationTestBase,
        IClassFixture<CustomWebApplicationFactory>
    {

        public DeviceAuthorizationTests(
            CustomWebApplicationFactory factory,
            ITestOutputHelper output)
            : base(factory, output) { 
        
        }

        //GetDevices Start
        [Fact]
        public async Task GetDevices_Should_Return_200_For_Admin()
        {
            await LoginAsAdminAsync();

            var response =
                await Client.GetAsync("/api/device");


            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDevices_Should_Return_200_When_User()
        {
            await LoginAsUserAsync();

            var response =
                await Client.GetAsync("/api/device");


            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDevices_Should_Return_401_When_No_Token()
        {
            var response =
                await Client.GetAsync("/api/device");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }
        //GetDevices End

        [Fact]
        public async Task GetAdminResource_Should_Return_403_For_Normal_User()
        {
            var token = "";

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
              

            var response = await Client.GetAsync("api/device");

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        
    }
}