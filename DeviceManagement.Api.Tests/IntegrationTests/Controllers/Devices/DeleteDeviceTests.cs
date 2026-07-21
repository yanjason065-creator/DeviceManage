using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DeviceManagement.Api.Data;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Tests.Infrastructure;
using DeviceManagement.Api.Tests.Fixtures;
using Xunit.Abstractions;
using Microsoft.VisualBasic;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Tests.Helpers;
using DeviceManagement.Api.Tests.Models;
using DeviceManagement.Api.Tests.TestData;
using DeviceManagement.Api.Tests.Assertions;

namespace DeviceManagement.Api.Tests.IntegrationTests.Controllers.Devices
{
    public class DeleteDeviceTests : 
        IntegrationTestBase,
        IClassFixture<CustomWebApplicationFactory>
    {
        private readonly ITestOutputHelper output;
        private const string DeviceUrl = "/api/device";

        public DeleteDeviceTests(
            CustomWebApplicationFactory factory, 
            ITestOutputHelper output) : base(factory, output)
        {

        }

        [Fact]
        public async Task Delete_When_AdminDeletesDevice_ShouldReturn200()
        {
            //Arrange
            await LoginAsAdminAsync();

            var create = await DeviceHelper.CreateDeviceAsync();

            var deleteId = create.Id;

            var before = await Database.GetDeviceAsync(deleteId);
            //Act
            var response = await Client.DeleteAsync($"{DeviceUrl}/{deleteId}");

            //Assert Http
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            result.Success.Should().BeTrue();

            result.Data.Should().Contain("Deleted ");


            //Assert Database
            var after = await Database.GetDeviceIngnoreFilterAsync(deleteId);

            DeviceAssertions.ShouldBeDeleted(before!, after!);
        }

        [Fact]
        public async Task Delete_When_NoToken_ShouldReturn401()
        {
            //Arrange
            await LoginAsAdminAsync();

            var create = await DeviceHelper.CreateDeviceAsync();

            var deleteId = create.Id;

            var before = await Database.GetDeviceAsync(deleteId);

            Logout();
            //Act
            var response = await Client.DeleteAsync($"{DeviceUrl}/{deleteId}");

            //Assert Http
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
         

            //Assert Database
            var after = await Database.GetDeviceIngnoreFilterAsync(deleteId);

            DeviceAssertions.ShouldNotBeChanged(before!, after!); 

        }

        [Fact]
        public async Task Delete_When_UserDeletesDevice_ShouldReturn403()
        {
            await LoginAsAdminAsync();

            var create = await DeviceHelper.CreateDeviceAsync();

            var deleteId = create.Id;

            var before = await Database.GetDeviceAsync(deleteId);

            Logout();

            await LoginAsUserAsync();
            //Act
            var response = await Client.DeleteAsync($"{DeviceUrl}/{deleteId}");

            //Assert Http
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);


            //Assert Database
            var after = await Database.GetDeviceIngnoreFilterAsync(deleteId);

            DeviceAssertions.ShouldNotBeChanged(before!, after!);
        }

        [Fact]
        public async Task Delete_When_DeviceNotFound_ShouldReturn404()
        {
            await LoginAsAdminAsync();

            //Act
            const long nonExistingId = 999999;
            var response = await Client.DeleteAsync(
                $"{DeviceUrl}/{nonExistingId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = await response.Content
                .ReadFromJsonAsync<ValidationErrorResponse>();

            ApiResponseAssertions.ShouldBeFailed(result);

            result.Message.Should().Be(ErrorMessages.DeviceNotFound);                        
        }

        [Fact]
        public async Task Delete_When_DeviceAlreadyDeleted_ShouldReturn404()
        {
            //Arrange
            await LoginAsAdminAsync();

            var create = await DeviceHelper.CreateDeviceAsync();

            var deleteId = create.Id;

            var before = await Database.GetDeviceAsync(deleteId);
            
            var responseA = await Client.DeleteAsync($"{DeviceUrl}/{deleteId}");

            responseA.StatusCode.Should().Be(HttpStatusCode.OK);

            var after1 = await Database.GetDeviceIngnoreFilterAsync(deleteId);

            DeviceAssertions.ShouldBeDeleted(before!, after1);

            //
            var response = await Client.DeleteAsync($"{DeviceUrl}/{deleteId}");

            //Assert Http
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = await response.Content
                .ReadFromJsonAsync<ValidationErrorResponse>();

            ApiResponseAssertions.ShouldBeFailed(result);

            result.Message.Should().Be(ErrorMessages.DeviceNotFound);

            var after2 = await Database.GetDeviceIngnoreFilterAsync(deleteId);

            DeviceAssertions.ShouldNotBeChanged(after1, after2!);
        }

        [Fact]
        public async Task Delete_When_DeviceDeleted_ShouldNotAppearInGetAll()
        {
            //Arrange
            await LoginAsAdminAsync();

            var create = await DeviceHelper.CreateDeviceAsync();

            //Act1
            var deleteId = create.Id;

            var before = await Database.GetDeviceAsync(deleteId);

            var deleteResponse = await Client.DeleteAsync($"{DeviceUrl}/{deleteId}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var devices = await DeviceHelper.GetDevicesAsync();

            devices.Items.Should()
                .NotContain(x=> x.Id == deleteId);
        }
    }
}
