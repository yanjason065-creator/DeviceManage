using DeviceManagement.Api.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Net.Http.Headers;
using DeviceManagement.Api.Tests.Clients;

namespace DeviceManagement.Api.Tests.Infrastructure
{
    public abstract class IntegrationTestBase
    {
        protected readonly HttpClient Client;
        protected readonly ITestOutputHelper Output;
        protected readonly CustomWebApplicationFactory Factory;
        protected DeviceApiClient DeviceApi;

        protected DatabaseHelper Database {  get; }
        protected DeviceTestHelper DeviceHelper { get; }

        protected IntegrationTestBase(CustomWebApplicationFactory factory, ITestOutputHelper output)
        {
            Factory = factory;
            Client = factory.CreateClient();
            Output = output;
            Database = new DatabaseHelper(factory.Services);
            DeviceHelper = new DeviceTestHelper(Client);
            DeviceApi = new DeviceApiClient(Client);
        }

        protected async Task<string> GetAdminTokenAsync()
        {
            return await AuthenticationHelper.GetAdminTokenAsync(Client);
        }

        protected async Task<string> GetUserTokenAsync()
        {
            return await AuthenticationHelper.GetUserTokenAsync(Client);
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
