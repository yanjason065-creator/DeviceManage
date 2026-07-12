namespace DeviceManagement.Api.Models
{
    public class Device
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public DeviceStatus Status { get; set; }

        public bool IsDeleted {  get; set; }

        public long CategoryId {  get; set; }

        public Category Category { get; set; } = null!;
        public long EmployeeId {  get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
