using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagement.Api.Tests.Helpers
{
    public static class TestDatabaseInitializer
    {
        public static void Initialize(
            IServiceProvider services            )
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();
        }
    }
}
