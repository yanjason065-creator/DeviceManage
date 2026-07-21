using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Api.DTOs
{
    public class DeviceDto
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength =2)]
        public string Name { get; set; }= string.Empty;
        [Required]
        public string  Status { get; set; } = string.Empty;

        public bool DeleteStatus { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CategoryName { get; set; } = string.Empty;

        public string UserName {  get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
