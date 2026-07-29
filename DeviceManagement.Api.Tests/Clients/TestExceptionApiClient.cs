using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Clients
{
    public class TestExceptionApiClient
    {
        private readonly HttpClient _client;

        public TestExceptionApiClient(HttpClient client)
        {
            _client = client;
        }


        public Task<HttpResponseMessage> ThrowValidationExceptionAsync()
        {
            return _client.GetAsync(
                "/api/test/exceptions/validation");
        }


        public Task<HttpResponseMessage> ThrowNotFoundExceptionAsync()
        {
            return _client.GetAsync(
                "/api/test/exceptions/notfound");
        }


        public Task<HttpResponseMessage> ThrowConflictExceptionAsync()
        {
            return _client.GetAsync(
                "/api/test/exceptions/conflict");
        }


        public Task<HttpResponseMessage> ThrowExceptionAsync()
        {
            return _client.GetAsync(
                "/api/test/exceptions/unknown");
        }
    }
}
