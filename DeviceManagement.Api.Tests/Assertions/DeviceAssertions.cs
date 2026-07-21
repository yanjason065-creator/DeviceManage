using DeviceManagement.Api.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Assertions
{
    public static class DeviceAssertions
    {
        public static void ShouldNotBeChanged(Device before, Device after)
        {
            after.Should().NotBeNull();

            after.Name.Should().Be(before.Name);

            after.Status.Should().Be(before.Status);

            after.EmployeeId.Should().Be(before.EmployeeId);

            after.CategoryId.Should().Be(before.CategoryId);

            after.IsDeleted.Should().Be(before.IsDeleted);

            after.CreatedAt.Should().Be(before.CreatedAt);

            after.UpdatedAt.Should().Be(before.UpdatedAt);
        }

        public static void ShouldBeUpdated(Device before, Device after)
        {
            after.Name.Should().NotBe(before.Name);

            after.UpdatedAt.Should().NotBeNull();

            after.UpdatedAt.Should().BeAfter(before.UpdatedAt ?? before.CreatedAt);
        }

        public static void ShouldBeDeleted(
            Device before, 
            Device after)
        {
            after.Should().NotBeNull();

            after.Name.Should().Be(before.Name);

            after.Status.Should().Be(before.Status);

            after.EmployeeId.Should().Be(before.EmployeeId);

            after.CategoryId.Should().Be(before.CategoryId);

            after.IsDeleted.Should().BeTrue();

            after.CreatedAt.Should().Be(before.CreatedAt);

            after.UpdatedAt.Should().NotBeNull();
        }
        
    }
}
