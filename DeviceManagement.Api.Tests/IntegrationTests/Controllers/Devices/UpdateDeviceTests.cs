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

namespace DeviceManagement.Api.Tests.IntegrationTests.Controllers.Devices
{
    public class UpdateDeviceTests
        :IntegrationTestBase,
        IClassFixture<CustomWebApplicationFactory>
    {
        private readonly ITestOutputHelper output;
        private const string DeviceUrl = "/api/device";
        
        public UpdateDeviceTests(
            CustomWebApplicationFactory factory, 
            ITestOutputHelper output) : base(factory, output)
        {
        }

        [Fact]
        public async Task UpdateDevice_WithAdminRole_ShouldReturnOk()
        {
            //Arrange
            await LoginAsAdminAsync();

            var create = await DeviceHelper.CreateDeviceAsync();
           

            var deviceId = create!.Id;

            var originalCreatedAt = create!.CreatedAt;

            var updateRequest = new UpdateDeviceDto
            {
                Name = $"QA Uodate Device {deviceId}",
                Status = DeviceStatus.Maintenance,
                EmployeeId = 2,
                CategoryId = 2,
                IsDeleted = false
            };

            var before = await Database.GetDeviceAsync(deviceId);

            //var response =
            //    await Client.PutAsJsonAsync(
            //        $"{DeviceUrl}/{deviceId}",
            //        updateRequest);

            var response = await DeviceApi.UpdateAsync(deviceId, updateRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<DeviceDto>>();
            
            ApiResponseAssertions.ShouldBeSuccessful(result);

            result.Data.Name.Should().Be(updateRequest.Name);

            result.Data.Status.Should().Be(updateRequest.Status.ToString());

            result.Data.CreatedAt.Should().Be(originalCreatedAt);

            result.Data.UpdatedAt.Should().NotBeNull();

            result.Data.UpdatedAt.Should().BeAfter(originalCreatedAt);

            //Database Verification
            var device = await Database.GetDeviceAsync(deviceId);

            DeviceAssertions.ShouldBeUpdated(before!, device!);

            device!.Name.Should().Be(updateRequest?.Name);

            device.Status.Should().Be(updateRequest.Status);

            device.UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateDevice_WithoutToken_ShouldReturnUnauthorized()
        {
            //Arrange -create test data
            await LoginAsAdminAsync();

            var device = await DeviceHelper.CreateDeviceAsync();

            Logout();
      

            var updateRequest = new UpdateDeviceDto
            {
                Name = "Updated",
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1
            };

            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    updateRequest);
            var response = await DeviceApi.UpdateAsync(device.Id, updateRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task UpdateDevice_WithUserRole_ShouldReturnForbidden()
        {
            //Arrange
            await LoginAsAdminAsync();

            var device = DeviceHelper.CreateDeviceAsync();

            Logout();

            await LoginAsUserAsync();

            var updateRequest = new UpdateDeviceDto
            {
                Name = "Updated",
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1
            };

            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    updateRequest);
            var response = await DeviceApi.UpdateAsync(device.Id, updateRequest);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task UpdateDevice_WithNonExistingId_ShouldReturnNotFound()
        {
            await LoginAsAdminAsync();

            var request = new UpdateDeviceDto
            {
                Name = "Updated Device",
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1,
                IsDeleted = false
            };

            

            //Act
            const long nonExistingId = 999999;
            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{nonExistingId}",
            //    request);
            var response = await DeviceApi.UpdateAsync(nonExistingId, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var result = await response.Content
                .ReadFromJsonAsync<ValidationErrorResponse>();

            ApiResponseAssertions.ShouldBeFailed(result);           

            result.Message.Should().Be(ErrorMessages.DeviceNotFound);
                       
        }

        [Fact]
        public async Task UpdateDevice_WithEmptyName_ShouldReturnBadRequest()
        {
            //Arrange
            await LoginAsAdminAsync();

            var device = await DeviceHelper.CreateDeviceAsync();

            var request = new UpdateDeviceDto
            {
                Name = "",
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1,
                IsDeleted = false
            };

            var before = await Database.GetDeviceAsync(device.Id);

            //Act
            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    request);
            var response = await DeviceApi.UpdateAsync(device.Id, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var validation =
                await response.Content
                     .ReadFromJsonAsync<ValidationErrorResponse>();

           //validation.Should().NotBeNull();
           ApiResponseAssertions.ShouldBeFailed(validation);

            ValidationAssertions
                .ShouldContainError(
                validation,
                "Name",
                "Device name is required.");

            ValidationAssertions
                .ShouldContainError(
                validation,
                "Name",
                "Device name must be between 2 and 50 characters.");

            var after = await Database.GetDeviceAsync(device.Id);
            DeviceAssertions.ShouldNotBeChanged(before!, after!);
        }


        //Name Validation, Too short or Too long
        [Theory]        
        [InlineData("A")]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")]
        public async Task UpdateDevice_WithInvalidName_ShouldReturnBadRequest(string name)
        {
            await LoginAsAdminAsync();

            var device = await DeviceHelper.CreateDeviceAsync();

            var request = new UpdateDeviceDto
            {
                Name = name,
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1,
                IsDeleted = false
            };

            var before = await Database.GetDeviceAsync(device.Id);
            //Act
            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    request);
            var response = await DeviceApi.UpdateAsync(device.Id, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var validation =
                await response.Content
                     .ReadFromJsonAsync<ValidationErrorResponse>();

            //validation.Should().NotBeNull();
            ApiResponseAssertions.ShouldBeFailed(validation);

            //ValidationAssertions
            //    .ShouldContainError(
            //    validation,
            //    "Name",
            //    "Device name is required.");

            ValidationAssertions
                .ShouldContainError(
                validation,
                "Name",
                "Device name must be between 2 and 50 characters.");

            var after = await Database.GetDeviceAsync(device.Id);
            DeviceAssertions.ShouldNotBeChanged(before!, after!);
        }

        [Fact]
        public async Task UpdateDevice_WithDuplicateName_ShouldReturnConflict()
        {
            //Arrange
            await LoginAsAdminAsync();

            var device1 = await DeviceHelper.CreateDeviceAsync();

            var device2 = await DeviceHelper.CreateDeviceAsync();

            //Act
            var request = new UpdateDeviceDto
            {
                Name = device1.Name,
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1,
                IsDeleted = false
            };

            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device2.Id}",
            //    request);
            var response = await DeviceApi.UpdateAsync(device2.Id, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<object>>();

            result!.Success.Should().BeFalse();

            result.Message.Should().Be(ErrorMessages.DeviceAlreadyExists);

            var device = await Database.GetDeviceAsync(device2.Id);
            device!.Name.Should().Be(device2.Name);
            device.UpdatedAt.Should().BeNull();

        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateDevice_WithInvalidEmployeeId_ShouldReturnBadRequest(int employeeId)
        {
            //Arrange
            await LoginAsAdminAsync();

            var device = await DeviceHelper.CreateDeviceAsync();

            var request = new UpdateDeviceDto
            {
                Name = device.Name,
                Status = DeviceStatus.Active,
                EmployeeId = employeeId,
                CategoryId = 1,
                IsDeleted = false
            };

            var before = await Database.GetDeviceAsync(device.Id);

            //Act
            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    request);
            var response = await DeviceApi.UpdateAsync(device.Id, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content
                .ReadFromJsonAsync<ValidationErrorResponse>();

            ValidationAssertions.ShouldContainError(
                result!,
                "EmployeeId",
                "'Employee Id' 必须大于 '0'。");

            var after = await Database.GetDeviceAsync(device.Id);

            DeviceAssertions.ShouldNotBeChanged(before!, after!);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task UpdateDevice_WithInvalidCategoryId_ShouldReturnBadRequest(int categoryId)
        {
            //Arrange
            await LoginAsAdminAsync();

            var device = await DeviceHelper.CreateDeviceAsync();

            var request = new UpdateDeviceDto
            {
                Name = device.Name,
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = categoryId,
                IsDeleted = false
            };

            var before = await Database.GetDeviceAsync(device.Id);

            //Act
            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    request);
            var response = await DeviceApi.UpdateAsync(device.Id, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content
                .ReadFromJsonAsync<ValidationErrorResponse>();

            ValidationAssertions.ShouldContainError(
                result!,
                "CategoryId",
                "'Category Id' 必须大于 '0'。");

            var after = await Database.GetDeviceAsync(device.Id);

            DeviceAssertions.ShouldNotBeChanged (before!, after!);
        }

        [Fact]
        public async Task UpdateDevice_WithNonExistingEmployee_ShouldReturnNotFound()
        {
            await LoginAsAdminAsync();

            var device = DeviceHelper.CreateDeviceAsync();

            var request = new UpdateDeviceDto
            {
                Name = "Update Device12",
                Status = DeviceStatus.Active,
                EmployeeId = 999999,
                CategoryId = 1,
                IsDeleted = false
            };

            var before = await Database.GetDeviceAsync(device.Id);

            //Act
            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    request);
            var response = await DeviceApi.UpdateAsync(device.Id, request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = await response.Content
                .ReadFromJsonAsync<ValidationErrorResponse>();

            ApiResponseAssertions.ShouldBeFailed(result);

            result.Message.Should().Be(ErrorMessages.EmployeeNotFound);

            var after = await Database.GetDeviceAsync(device.Id);

            DeviceAssertions.ShouldNotBeChanged(before!, after!);
        }

        [Fact]
        public async Task UpdateDevice_WithNonExistingCategory_ShouldReturnNotFound()
        {
            await LoginAsAdminAsync();

            var device = DeviceHelper.CreateDeviceAsync();

            var request = new UpdateDeviceDto
            {
                Name = "Update Device12",
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 999999,
                IsDeleted = false
            };

            var before = await Database.GetDeviceAsync(device.Id);
            //Act
            //var response = await Client.PutAsJsonAsync(
            //    $"{DeviceUrl}/{device.Id}",
            //    request);
            var response = await DeviceApi.UpdateAsync(device.Id, request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var result = await response.Content
                .ReadFromJsonAsync<ValidationErrorResponse>();

            ApiResponseAssertions.ShouldBeFailed(result);

            result.Message.Should().Be(ErrorMessages.CategoryNotFound);

            var after = await Database.GetDeviceAsync (device.Id);

            DeviceAssertions.ShouldNotBeChanged(before!, after!);
          
            
        }
    }
}
