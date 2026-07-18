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
using DeviceManagement.Api.Tests.Helpers.TestHelper;

namespace DeviceManagement.Api.Tests.IntegrationTests.Controllers.Devices
{
    public class CreateDeviceTests
        :IntegrationTestBase,
        IClassFixture<CustomWebApplicationFactory>
    {
        
        private readonly ITestOutputHelper _output;
        private const string DeviceUrl = "/api/device";

        private static CreateDeviceDto CreateValidRequest()
        {
            return new CreateDeviceDto
            {
                Name = $"QA Device {Guid.NewGuid()}",
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1,
            };
        }

        public CreateDeviceTests(CustomWebApplicationFactory factory
            ,ITestOutputHelper output)
            :base(factory, output)
        {
            
        }

        [Fact]
        public async Task CreateDevice_WithAdminRole_ShouldReturnCreated()
        {                        
            //Arrange
            await LoginAsAdminAsync();

            var request = CreateValidRequest();

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result =
                await response.Content
                .ReadFromJsonAsync<ApiResponse<DeviceDto>>();

            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

          


            result.Should().NotBeNull();

            result!.Success.Should().BeTrue();

            result.Data.Should().NotBeNull();

            result.Data.Name.Should().Be(request.Name);

            result.Data.Status.Should().Be(request.Status.ToString());

            result.Data.UserName.Should().NotBeNullOrWhiteSpace();

            result.Data.CategoryName.Should().NotBeNullOrWhiteSpace();

            var device = await db.Devices
                .Include(x => x.Employee)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == result.Data.Id);

            device.Should().NotBeNull();

            device!.Name.Should().Be(request.Name);

            device.Status.Should().Be(request.Status);

            device.EmployeeId.Should().Be(request.EmployeeId);

            device.CategoryId.Should().Be(request.CategoryId);

            device.Employee.Name.Should().Be(result.Data.UserName);

            device.Category.Name.Should().Be(result.Data.CategoryName);

        }

        //Case2
        [Fact]
        public async Task CreateDevice_WithoutToken_ShouldReturnUnauthorized()
        {
            var request = CreateValidRequest();

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        //Case3
        [Fact]
        public async Task CreateDevice_WithUserRole_ShouldReturnForbidden()
        {
            await LoginAsUserAsync();

            var request =CreateValidRequest();

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateDevice_WithEmptyName_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request =CreateValidRequest();
            request.Name = "";

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            //result.Should().NotBeNull();

            //result!.Success.Should().BeFalse();

            //result.Message.Should().Be("Validation failed");

            //result.Data.Should().ContainKey("Name");

            //result.Data["Name"].Should()
            //    .Contain("Device name is required.");

            await ValidationAssertions.ShouldHaveValidationError(
                response,
                "Name",
                "Device name is required.");
        }
        [Fact]
        public async Task CreateDevice_WithTooShortName_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request =CreateValidRequest();
            request.Name = "A";

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            //Status 
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);

            //Response
            //var result = await response.Content
            //    .ReadFromJsonAsync<ValidationErrorResponse>();
            //result.Should().NotBeNull();
            //result!.Success.Should().BeFalse();
            //result.Message.Should().Be("Validation failed");
            //result.Data?.Should().ContainKey("Name");
            //result.Data["Name"]
            //    .Should()
            //    .Contain("Device name must be between 2 and 50 characters.");

            await ValidationAssertions.ShouldHaveValidationError(
                response,
                "Name",
                "Device name must be between 2 and 50 characters.");
        }
        [Fact]
        public async Task CreateDevice_WithTooLongName_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request =CreateValidRequest();
            request.Name = new string('A', 51);

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            //Status
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //Response
            //var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            //result.Should().NotBeNull();
            //result!.Success.Should().BeFalse();
            //result.Message.Should().Be("Validation failed");
            //result.Data?.Should().ContainKey("Name");
            //result.Data["Name"]
            //    .Should()
            //    .Contain("Device name must be between 2 and 50 characters.");

            await ValidationAssertions.ShouldHaveValidationError(
                response,
                "Name",
                "Device name must be between 2 and 50 characters.");

        }

        [Fact]
        public async Task CreateDevice_WithInvalidStatus_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request =CreateValidRequest();
            request.Status = (DeviceStatus)999;

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            result?.Data["Status"].Should().NotBeNull();
        }

        [Fact]
        public async Task CreateDevice_WithInvalidEmployeeId_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request =CreateValidRequest();
            request.EmployeeId = 0;

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            result?.Data.Should()
                .ContainKey("EmployeeId");
        }

        [Fact]
        public async Task CreateDevice_WithNonExistingEmployee_ShouldReturnNotFound()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request =CreateValidRequest();
            request.EmployeeId = 999;

        
            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.NotFound);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            string expectMessage = "Employee Not Found.";
            result?.Message.Should()
                .Be(expectMessage);
  
        }
        [Fact]
        public async Task CreateDevice_WithInvalidCategoryId_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request = CreateValidRequest();
            request.CategoryId = 0;

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            result?.Data.Should()
                .ContainKey("Category");
        }

        [Fact]
        public async Task CreateDevice_WithNonExistingCategory_ShouldReturnNotFound()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request = CreateValidRequest();
            request.CategoryId = 10;

            //Act
            var response = await Client.PostAsJsonAsync(
                DeviceUrl,
                request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.NotFound);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            string expectMessage = "Category Not Found.";
            result?.Message.Should()
                .Be(expectMessage);
        }
    }
}
