using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Tests.Models;
using FluentAssertions;

namespace DeviceManagement.Api.Tests.Assertions
{
    public static class ApiResponseAssertions
    {
        public static void ShouldBeSuccessful<T>(
            ApiResponse<T> response)
        {
            response.Should().NotBeNull();

            response.Success.Should().BeTrue(); 

            response.Data.Should().NotBeNull();
        }

        public static void ShouldBeFailed(ValidationErrorResponse response)
        {
            response.Should().NotBeNull();
            response.Success.Should().BeFalse();
            //response.Data.Should().NotBeNull();
            //response.Message.Should().Be("Validation failed");
        }
    }
}
