using DeviceManagement.Api.Data;
using DeviceManagement.Api.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DeviceManagement.Api.Tests.Infrastructure
{
   public class CustomWebApplicationFactory:
        WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            //builder.ConfigureServices(services =>
            //{
            //    var descriptor =
            //        services.SingleOrDefault(
            //            d =>
            //            d.ServiceType ==
            //            typeof(DbContextOptions<AppDbContext>)
            //         );

            //    if (descriptor != null)
            //    {
            //        services.Remove(descriptor);
            //    }

            //    services.RemoveAll<AppDbContext>();

            //    services.RemoveAll<DbContextOptions<AppDbContext>>();

            //    services.AddDbContext<AppDbContext>(
            //        options =>
            //        {
            //            options.UseSqlite(

            //                "DataSource=:memory:"
            //             );
            //        });
            //});

            //using(var scope = Services.CreateScope())
            //{
            //    TestDatabaseInitializer.Initialize(Services);
            //}
        }
    }
}
