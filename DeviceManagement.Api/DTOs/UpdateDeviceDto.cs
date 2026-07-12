using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.DTOs
{
    public class UpdateDeviceDto
    {
        //[Required]
        //[StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public DeviceStatus Status { get; set; }

        public bool IsDeleted { get; set; }
        public int EmployeeId { get; set; }
        
        public int CategoryId { get; set; }
    }
}
