using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DeviceManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {

        private readonly IDeviceService _deviceService;
        private readonly ILogger<DeviceController> _logger;

        public DeviceController(IDeviceService deviceService, ILogger<DeviceController> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
            //_logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetDevices([FromQuery] DeviceQuery query)
        {
            // return Ok("Device API is working");

            //return Ok(_deviceService.GetDevices(query));
            _logger.LogInformation("Getting devices with query: {@query}", query);
            return Ok(ApiResponse<PagedResults<DeviceDto>>.Ok(_deviceService.GetDevices(query)));
       
        }

        [HttpPost]
        [Authorize(Roles =Roles.Admin)]
        public IActionResult CreateDevice(CreateDeviceDto dto)
        {
            _logger.LogInformation("CreateDevice started");
            //return Ok(_deviceService.AddDevice(dto));
            return Ok(ApiResponse<DeviceDto>.Ok(_deviceService.AddDevice(dto)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult UpdateDevice(long id, UpdateDeviceDto dto)
        {

            var result = _deviceService.UpdateDevice(id, dto);
            if (null == result)
            {
                return NotFound();
            }
            return Ok(ApiResponse<DeviceDto>.Ok(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteDevice(long id) {
            var result  = _deviceService.DeleteDevice(id);
            if (!result) {
                return NotFound(ApiResponse<string>.Fail("Device Not Found"));
            }

            return Ok(ApiResponse<string>.Ok("Deleted Successfully"));
        }
    }
}
