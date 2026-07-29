using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Tests.Clients;
using DeviceManagement.Api.Tests.Infrastructure;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DeviceManagement.Api.Tests.Middleware
{
    public class ExceptionHandlingMiddlewareTests
        : IntegrationTestBase,
        IClassFixture<CustomWebApplicationFactory>
    {
        private readonly ITestOutputHelper _output;
        private const string DeviceUrl = "/api/device";
        private readonly TestExceptionApiClient _exceptionClient;

        public ExceptionHandlingMiddlewareTests(
            CustomWebApplicationFactory factory,
            ITestOutputHelper testOutputHelper)
            :base(factory, testOutputHelper) 
        {
            _exceptionClient = new TestExceptionApiClient(Client);
        }

        [Fact]
        public async Task ValidationException_Should_Return400()
        {
            var response =
                await _exceptionClient.ThrowValidationExceptionAsync();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            var body =
                await response.Content
                .ReadFromJsonAsync<ApiResponse<object>>();

            body!.Success.Should().BeFalse();

        }

        [Fact]
        public async Task NotFoundException_Should_Return404()
        {
            var response =
                await _exceptionClient.ThrowNotFoundExceptionAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task ConflictException_Should_Return409()
        {
            var response =
                await _exceptionClient.ThrowConflictExceptionAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Conflict);
        }


        [Fact]
        public async Task UnknownException_Should_Return500()
        {
            var response =
                await _exceptionClient.ThrowExceptionAsync();

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.InternalServerError);
        }
    }
}
