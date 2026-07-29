using DeviceManagement.Api.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Net.Http.Headers;
using DeviceManagement.Api.Tests.Clients;
using System.Net.Http.Json;
using DeviceManagement.Api.DTOs.Auth;

namespace DeviceManagement.Api.Tests.Infrastructure
{
    [Collection("Integration Test Collection")]
    public abstract class IntegrationTestBase
    {
        protected  HttpClient Client { get; }
        protected ITestOutputHelper Output { get; }
        protected CustomWebApplicationFactory Factory { get; }
        protected DeviceApiClient DeviceApi { get; }

        protected DatabaseHelper Database {  get; }
        protected DeviceTestHelper DeviceHelper { get; }
        protected AuthApiClient AuthApi { get; }

        protected IntegrationTestBase(CustomWebApplicationFactory factory, ITestOutputHelper output)
        {
            Factory = factory;
            Client = factory.CreateClient();
            Output = output;
            Database = new DatabaseHelper(factory.Services);
            DeviceHelper = new DeviceTestHelper(Client);
            DeviceApi = new DeviceApiClient(Client);
            AuthApi = new AuthApiClient(Client);
        }

        protected async Task<string> GetAdminTokenAsync()
        {
            //return await AuthenticationHelper.GetAdminTokenAsync(Client);
            var request = AuthenticationHelper.AdminLog();

            var response = await AuthApi.LoginAsync(request);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            return result!.Token;
            
        }

        protected async Task<string> GetUserTokenAsync()
        {
            //return await AuthenticationHelper.GetUserTokenAsync(Client);
            var request = AuthenticationHelper.UserLogin();

            var response = await AuthApi.LoginAsync(request);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            return result!.Token;
        }

        protected async Task LoginAsAdminAsync()
        {
            var token = await GetAdminTokenAsync();

            Client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
        }

  

        protected async Task LoginAsUserAsync()
        {
            var token = await GetUserTokenAsync();

            Client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
        }

        protected void Logout()
        {
            Client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
