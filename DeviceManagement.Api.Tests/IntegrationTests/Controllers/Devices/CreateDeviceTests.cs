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
using Xunit.Abstractions;
using Microsoft.VisualBasic;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Tests.Helpers;
using DeviceManagement.Api.Tests.Models;
using DeviceManagement.Api.Tests.Assertions;
using DeviceManagement.Api.Tests.Builders;

namespace DeviceManagement.Api.Tests.IntegrationTests.Controllers.Devices
{
    public class CreateDeviceTests
        :IntegrationTestBase,
        IClassFixture<CustomWebApplicationFactory>
    {
        
        private readonly ITestOutputHelper _output;
        private const string DeviceUrl = "/api/device";


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

            //var request = DeviceTestData.CreateValidRequest();

            //var request = new DeviceBuilder().BuildCreateDto();
            var request = DeviceBuilder.Default().BuildCreateDto();

            var before = DateTime.UtcNow;
            //Act
          
            var response = await DeviceApi.CreateAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var result =
                await response.Content
                .ReadFromJsonAsync<ApiResponse<DeviceDto>>();

           

          
            var after = DateTime.UtcNow;


            ApiResponseAssertions.ShouldBeSuccessful(result!);

            result?.Data?.Name.Should().Be(request.Name);

            result?.Data?.Status.Should().Be(request.Status.ToString());

            result?.Data?.UserName.Should().NotBeNullOrWhiteSpace();

            result?.Data?.CategoryName.Should().NotBeNullOrWhiteSpace();

            var device = await Database.GetDeviceAsync(result!.Data!.Id);
           

            device.Should().NotBeNull();

            device!.Name.Should().Be(request.Name);

            device.Status.Should().Be(request.Status);

            device.EmployeeId.Should().Be(request.EmployeeId);

            device.CategoryId.Should().Be(request.CategoryId);

            device.Employee.Name.Should().Be(result.Data.UserName);

            device.Category.Name.Should().Be(result.Data.CategoryName);

            device.CreatedAt.Should().BeOnOrAfter(before);

            device.CreatedAt.Should().BeOnOrBefore(after);

            device.UpdatedAt.Should().BeNull();

            result.Data.CreatedAt
                .Should()
                .Be(device.CreatedAt);
        }

        //Case2
        [Fact]
        public async Task CreateDevice_WithoutToken_ShouldReturnUnauthorized()
        {
            var request = DeviceTestData.CreateValidRequest();

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        //Case3
        [Fact]
        public async Task CreateDevice_WithUserRole_ShouldReturnForbidden()
        {
            await LoginAsUserAsync();

            var request = DeviceTestData.CreateValidRequest();

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateDevice_WithEmptyName_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            //var request = DeviceTestData.CreateValidRequest(); 

            var request = DeviceBuilder.Default().WithName("").BuildCreateDto();
            request.Name = "";

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);
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
            var request = DeviceTestData.CreateValidRequest(); 
            request.Name = "A";

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

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
            var request = DeviceTestData.CreateValidRequest(); 
            request.Name = new string('A', 51);

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

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
            var request = DeviceTestData.CreateValidRequest(); 
            request.Status = (DeviceStatus)999;

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

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
            var request = DeviceTestData.CreateValidRequest(); 
            request.EmployeeId = 0;

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);
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
            var request = DeviceTestData.CreateValidRequest(); 
            request.EmployeeId = 999;


            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.NotFound);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            
            result?.Message.Should()
                .Be(ErrorMessages.EmployeeNotFound);
  
        }
        [Fact]
        public async Task CreateDevice_WithInvalidCategoryId_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request = DeviceTestData.CreateValidRequest(); 
            request.CategoryId = 0;

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            result?.Data.Should()
                .ContainKey("CategoryId");
        }

        [Fact]
        public async Task CreateDevice_WithNonExistingCategory_ShouldReturnNotFound()
        {
            //Arrange
            await LoginAsAdminAsync();
            var request = DeviceTestData.CreateValidRequest(); 
            request.CategoryId = 10;

            //Act
            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var response = await DeviceApi.CreateAsync(request);

            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.NotFound);
            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            
            result?.Message.Should()
                .Be(ErrorMessages.CategoryNotFound);
        }

        [Fact]
        public async Task CreateDevice_WithDuplicateName_ShouldReturnConflict()
        {
            //Arrange
            await LoginAsAdminAsync();

            var request = DeviceTestData.CreateValidRequest();

            //Act
            //var firstResponse = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    request);
            var firstResponse = await DeviceApi.CreateAsync(request);

            firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var duplicateRequest = DeviceTestData.CreateValidRequest();
            duplicateRequest.Name = request.Name;

            var countBefore = await Database.CountDevicesAsync();

            //var response = await Client.PostAsJsonAsync(
            //    DeviceUrl,
            //    duplicateRequest);
            var response = await DeviceApi.CreateAsync(request);

            var countAfter = await Database.CountDevicesAsync();

            countBefore.Should().Be(countAfter);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<object>>();

            result!.Success.Should().BeFalse();
            result.Message.Should().Be(ErrorMessages.DeviceAlreadyExists);
        }
    }
}
