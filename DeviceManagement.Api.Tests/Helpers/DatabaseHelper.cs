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

        public  async Task<Device?> GetDeviceAsync(int id)
        {
            using var scope = 
                _serviceProvider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext> ();

            return await db.Devices.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
