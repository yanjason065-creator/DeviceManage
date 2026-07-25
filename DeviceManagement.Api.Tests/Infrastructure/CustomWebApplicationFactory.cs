using DeviceManagement.Api.Data;
using DeviceManagement.Api.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
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
        private readonly SqliteConnection _connection;

        public CustomWebApplicationFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }
        protected override void ConfigureWebHost(
       IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");


            builder.ConfigureServices(services =>
            {
                services.RemoveAll<
                    DbContextOptions<AppDbContext>>();


                services.AddDbContext<AppDbContext>(
                    options =>
                    {
                        options.UseSqlite(_connection);
                    });


                var serviceProvider =
                    services.BuildServiceProvider();


                TestDatabaseInitializer.Initialize(
                    serviceProvider);
            });
        }
    }
}
