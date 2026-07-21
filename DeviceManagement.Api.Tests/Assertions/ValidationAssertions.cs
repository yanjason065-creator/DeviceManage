using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using DeviceManagement.Api.Tests.Models;
namespace DeviceManagement.Api.Tests.Assertions
{
    public static class ValidationAssertions
    {
        public static async Task ShouldHaveValidationError(
            HttpResponseMessage response,
            string field,
            string expectedMessage)
        {
            

            var result = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

            result.Should().NotBeNull();
            result!.Success.Should().BeFalse();
            result.Message.Should().Be("Validation failed");
            result.Data.Should().ContainKey(field);
            result.Data[field]
                .Should()
                .Contain(expectedMessage);
        }

        public static void ShouldContainError(
            ValidationErrorResponse result,
            string field,
            string message)
        {
            result.Data[field].Should().Contain(message);
        }
    }
}
