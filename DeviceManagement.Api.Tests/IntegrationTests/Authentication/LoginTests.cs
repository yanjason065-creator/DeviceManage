using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DeviceManagement.Api.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using DeviceManagement.Api.Tests.Helpers;
using System.Security.Claims;
using Xunit.Abstractions;

using DeviceManagement.Api.Tests.Infrastructure;

namespace DeviceManagement.Api.Tests.Intergration.Authentication
{
    public  class LoginTests
        :IntegrationTestBase,
         IClassFixture<CustomWebApplicationFactory>
    {
        //private readonly HttpClient _client;
        //private readonly ITestOutputHelper _output;

        public LoginTests(CustomWebApplicationFactory factory,
                ITestOutputHelper testOutputHelper):base(factory,testOutputHelper)
        {            
        }

        [Fact]
        public async Task Login_Should_Return_Toekn_When_Credentials_Are_Valid()
        {
            //Arrange
            var request = new
            {
                username = "admin",
                password = "123456"
            };

            //Act
            var response =
                await Client.PostAsJsonAsync(
                    "/api/auth/login",
                    request
                    );
            Output.WriteLine(
                $"Status Code: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception( error );
            }
            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            Output.WriteLine(content);
            content
                .Should()
                .Contain("token");
        }

        [Fact]
        public async Task Login_Should_Return_401_When_Password_Is_Invalid()
        {
            //Arrange
            var request = new
            {
                username = "admin",
                password = "WrongPassword"
            };

            //Act
            var response =
                await Client.PostAsJsonAsync(
                    "/api/auth/login",
                    request
                    );
            Output.WriteLine(
                  $"Status Code: {response.StatusCode}");
            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_Should_Return_401_When_User_Does_Not_Exist()
        {
            //Arrange
            var request = new
            {
                username = "nobody",
                password = "WrongPassword"
            };

            //Act
            var response =
                await Client.PostAsJsonAsync(
                    "/api/auth/login",
                    request
                    );
            Output.WriteLine(
                $"Status Code: {response.StatusCode}");
            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_Should_Return_400_When_Request_Is_Invalid()
        {
            //Arrange
            var request = new
            {
                username = "",
                password = ""
            };

            //Act
            var response =
                await Client.PostAsJsonAsync(
                    "/api/auth/login",
                    request
                    );
            Output.WriteLine(
                $"Status Code: {response.StatusCode}");
            //Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_Should_Return_Valid_Jwt_Token_When_Credentials_Are_Valid()
        {
            //Arrange
            var request = new
            {
                username = "admin",
                password = "123456"
            };

            //Act
            var response =
                await Client.PostAsJsonAsync(
                    "/api/auth/login",
                    request
                    );
            Output.WriteLine(
                $"Status Code: {response.StatusCode}");
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<LoginResponseDto>();

            result.Should().NotBeNull();

            result!.Token
                .Should()
                .NotBeNullOrWhiteSpace();

            var jwt = JwtHelper.Decode(result.Token);
            jwt.Claims.Should()
                .Contain(c =>
                c.Type == ClaimTypes.Role &&
                c.Value == "Admin");

            jwt.Claims.Should()
                .Contain(c =>
                c.Type == ClaimTypes.Name &&
                c.Value == "admin");

            jwt.Claims.Should()
                .Contain(c =>
                c.Type == ClaimTypes.NameIdentifier &&
                c.Value == "1");
            jwt.Issuer.Should()
                .Be("DeviceManagement.Api");

            jwt.Audiences.Should()
                .Contain("DeviceManagement.Client");
            
        }
    }
}
