using DeviceManagement.Api.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Builders
{
    public class DeviceBuilder
    {
        private string _name;
        private DeviceStatus _status;
        private int _employeeId;
        private int _categoryId;
        private bool _isDeleted;

        private DeviceBuilder()
        {
            _name = $"QA Device {Guid.NewGuid()}";
            _status = DeviceStatus.Active;
            _employeeId = 1;
            _categoryId = 1;
            _isDeleted = false;
        }

        public DeviceBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public DeviceBuilder WithStatus(DeviceStatus status)
        {
            _status = status;
            return this;
        }

        public DeviceBuilder WithEmployeeId(int employeeId)
        {
            _employeeId = employeeId;
            return this;
        }

        public DeviceBuilder WithCategoryId(int categoryId)
        {
            _categoryId = categoryId;
            return this;
        }

        public DeviceBuilder WithIsDeleted(bool isDeleted)
        {
            _isDeleted = isDeleted;
            return this;
        }

        public CreateDeviceDto BuildCreateDto()
        {
            return new CreateDeviceDto
            {
                Name = _name,
                Status = _status,
                EmployeeId = _employeeId,
                CategoryId = _categoryId
            };

        }

        public UpdateDeviceDto BuildUpdateDto()
        {
            return new UpdateDeviceDto
            {
                Name = _name,
                Status = _status,
                EmployeeId = _employeeId,
                CategoryId = _categoryId,
                IsDeleted = _isDeleted
            };
        }

        public static DeviceBuilder Default()
        {
            return new DeviceBuilder();
        }
    }
}
