using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using DeviceManagement.Api.Tests.Models;
namespace DeviceManagement.Api.Tests.Helpers.TestHelper
{
    public static class ValidationAssertions
    {
        public static async Task ShouldHaveValidationError(
            HttpResponseMessage response,
            string field,
            string expectedMessage)
        {
            response.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            result.Should().NotBeNull();
            result!.Success.Should().BeFalse();
            result.Message.Should().Be("Validation failed");
            result.Data.Should().ContainKey(field);
            result.Data[field]
                .Should()
                .Contain(expectedMessage);
        }
    }
}
