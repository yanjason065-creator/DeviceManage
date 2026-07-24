using DeviceManagement.Api.DTOs;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Helpers
{
    public static class DeviceTestData
    {
        public static CreateDeviceDto CreateValidRequest()
        {
            return new CreateDeviceDto
            {
                Name = $"QA Device {Guid.NewGuid()}",
                Status = DeviceStatus.Active,
                EmployeeId = 1,
                CategoryId = 1,
            };
        }

    }
}
