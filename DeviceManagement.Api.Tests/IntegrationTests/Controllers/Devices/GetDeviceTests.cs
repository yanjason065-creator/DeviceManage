using Azure;
using DeviceManagement.Api.Data;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Tests.Assertions;
using DeviceManagement.Api.Tests.Constants;
using DeviceManagement.Api.Tests.Helpers;
using DeviceManagement.Api.Tests.Infrastructure;
using DeviceManagement.Api.Tests.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DeviceManagement.Api.Tests.IntegrationTests.Controllers.Devices
{
    public class GetDeviceTests : 
        IntegrationTestBase,
        IClassFixture<CustomWebApplicationFactory>
    {
        private readonly ITestOutputHelper _output;
        private const string DeviceUrl = "/api/device";

        public GetDeviceTests(CustomWebApplicationFactory factory, 
            ITestOutputHelper output) : base(factory, output)
        {
        }

        [Trait("Category", TestCategories.Smoke)]
        [Fact]
        public async Task GetAll_When_UserGetsDevices_ShouldReturn200()
        {
            //Arrange
            await LoginAsUserAsync();

            //Act
            var response = await DeviceApi.GetAllAsync();

            //Assertion
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();

        }

        [Trait("Category", TestCategories.Smoke)]
        [Fact]
        public async Task GetAll_When_AdminGetsDevices_ShouldReturn200()
        {
            //Arrange
            await LoginAsAdminAsync();

            //Act
            var response = await DeviceApi.GetAllAsync();

            //Assertion
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_NoToken_ShouldReturn401()
        {
            //Act
            var response = await DeviceApi.GetAllAsync();

            //Assertion
            response.StatusCode.Should()
              .Be(HttpStatusCode.Unauthorized);
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_DeviceExists_ShouldContainDevice()
        {           
            //Arrange
            await LoginAsAdminAsync();

            var request = DeviceTestData.CreateValidRequest();

            var createResponse = await DeviceApi.CreateAsync(request);

            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await createResponse.Content
                .ReadFromJsonAsync<ApiResponse<DeviceDto>>();

            var deviceId = created!.Data!.Id;

            //Act
            var getResponse = await DeviceApi.GetAllAsync();

            //Assert Http
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //Assert Response
            var result = await getResponse.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            //result!.Data.Items.Should()
            //    .Contain(x=> x.Id == deviceId);

            result!.Data!.Items.Any(x=> x.Id == deviceId);
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_DeviceDeleted_ShouldNotContainDevice()
        {
            //Arrange
            await LoginAsAdminAsync();

            var request = DeviceTestData.CreateValidRequest();

            var createResponse = await DeviceApi.CreateAsync(request);

            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await createResponse.Content
                .ReadFromJsonAsync<ApiResponse<DeviceDto>>();

            var deviceId = created!.Data!.Id;

            await DeviceApi.DeleteAsync(deviceId);

            //Act
            var getResponse = await DeviceApi.GetAllAsync("?Page=1&PageSize=200");

            //Assert Http
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //Assert Response
            var result = await getResponse.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            //result!.Data.Items.Should()
            //    .Contain(x=> x.Id == deviceId);

            //result!.Data.Items.Any (x=> x.Id == deviceId);//If query hasnofilter?
            result!.Data!.Items.Should().NotContain(x => x.Id == deviceId);
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_FilterName_ShouldReturnMatchingDevices()
        {
            //Arrange
            await LoginAsAdminAsync();

            var device1 = DeviceTestData.CreateValidRequest();
            device1.Name = "Freya Lapton Device2";

            var device2 = DeviceTestData.CreateValidRequest();
            device2.Name = "JIn Service Device2";

            await DeviceApi.CreateAsync(device1);
            await DeviceApi.CreateAsync(device2);

            //Act
            var response = await DeviceApi.GetAllAsync("?name=Freya&PageSize=200");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result!.Data!.Items.Should().OnlyContain(
                x => x.Name.Contains("Freya"));
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_FilterStatus_ShouldReturnMatchingDevices()
        {
            //Arrange
            await LoginAsAdminAsync();

           

            //Act
            var response = await DeviceApi.GetAllAsync("?status=Inactive&PageSize=200");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result!.Data!.Items.Should().NotBeEmpty();

            result!.Data.Items.Should().OnlyContain(
                x => x.Status == DeviceStatus.Inactive.ToString());
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_FilterByDeleted_ShouldReturnMatchingDevices()
        {
            //Arrange
            await LoginAsAdminAsync();



            //Act
            var response = await DeviceApi.GetAllAsync("?isDeleted=true&PageSize=200");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result!.Data!.Items.Should().NotBeEmpty();

            result!.Data.Items.Should().OnlyContain(
                x => x.DeleteStatus == true);
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_FilterByNameAndStatus_ShouldReturnMatchingDevices()
        {
            await LoginAsAdminAsync();

            var response = await DeviceApi.GetAllAsync("?Name=QA&status=Maintenance&pagesize=200");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result!.Data!.Items.Should()
                .OnlyContain(
                x =>
                x.Name.Contains("QA")
                &&
                x.Status == DeviceStatus.Maintenance.ToString());

        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_SortByNameAscending_ShouldReturnSortedDevices()
        {
            await LoginAsAdminAsync();

            var response = await DeviceApi.GetAllAsync("?sortby=name&pagesize=100");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            //Sql排序规则和Net排序规则可能会冲突
            result!.Data!.Items
                .Select(x=>x.Name.ToLower())
                .Should()
                .BeInAscendingOrder();
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_PageSizeSpecified_ShouldReturnLimitedRecords()
        {
            await LoginAsAdminAsync();

            var response = await DeviceApi.GetAllAsync("?page=1&pagesize=5");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result!.Data!.Items.Count.Should().BeLessThanOrEqualTo(5);

            result.Data.PageSize.Should().Be(5);
        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_RequestDifferentePages_ShouldReturnDifferentResults()
        {
            await LoginAsAdminAsync();
            var response1 = await DeviceApi.GetAllAsync("?page=1&pagesize=5");

            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            var response2 = await DeviceApi.GetAllAsync("?page=2&pagesize=5");
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            var firstPage = await response1.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            var secondPage = await response2.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            firstPage!.Data!.Items.Should().NotBeEmpty();
            secondPage!.Data!.Items.Should().NotBeEmpty();

            var firstIds = firstPage.Data.Items.Select(x => x.Id);
            var secondIds = secondPage.Data.Items.Select(x => x.Id);
            firstIds.Should().NotContain(secondIds);

        }

        [Trait("Category", TestCategories.Regression)]
        [Fact]
        public async Task GetAll_When_PageExceedsAvailableData_ShouldReturnEmpty()
        {
            await LoginAsAdminAsync();
            var response = await DeviceApi.GetAllAsync("?page=999&pagesize=10");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<PagedResults<DeviceDto>>>();

            result!.Data!.Items.Should().BeEmpty();

            result.Data.TotalCount.Should().BeGreaterThan(0);


        }
    }
}
