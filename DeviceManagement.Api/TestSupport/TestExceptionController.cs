#if DEBUG
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Api.Services;
using FluentValidation;


namespace DeviceManagement.Api.TestSupport
{
    [Route("api/test/exceptions")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestExceptionController : ControllerBase
    {
        [HttpGet("validation")]
        public IActionResult Validation()
        {
            throw new ValidationException("Test validation exception");
        }

        [HttpGet("notfound")]
        public IActionResult NotFoundException()
        {
            throw new NotFoundException("Device not found");

        }

        [HttpGet("conflict")]
        public IActionResult Conflict()
        {
            throw new ConflictException("Device already exists");
        }

        [HttpGet("unknown")]
        public IActionResult Unknown()
        {
            throw new Exception("Unexpected error");
        }
    }
}
#endif