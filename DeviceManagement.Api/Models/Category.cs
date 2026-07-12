namespace DeviceManagement.Api.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public List<Device> Devices { get; set; } = new();

        public bool IsDeleted { get; set; } = false;
    }
}
