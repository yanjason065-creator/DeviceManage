using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceManagement.Api.Data;
using DeviceManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DeviceManagement.Api.Tests.Helpers
{
    public class DatabaseHelper
    {
        private  readonly IServiceProvider _serviceProvider;

        public DatabaseHelper(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public  async Task<Device?> GetDeviceAsync(long id)
        {
            using var scope = 
                _serviceProvider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext> ();

            return await db.Devices
            .Include(x => x.Employee)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> CountDevicesAsync()
        {
            using var scope =
               _serviceProvider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            return await db.Devices.CountAsync();
        }

        public async Task<Device?> GetDeviceIngnoreFilterAsync(long id)
        {
            using var scope =
                _serviceProvider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            return await db.Devices
                .IgnoreQueryFilters()
                .Include(x => x.Employee)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
