using DeviceManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Api.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Device>  Devices { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<User> Users => Set<User>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Desktop" },
                new Category { Id = 3, Name = "Monitor" }
                );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Jason Yan" },
                new Employee { Id = 2, Name = "Freya Liu" }
              
                );

            // 1️⃣ Global Query Filter（软删除）
            modelBuilder.Entity<Device>()
                .HasQueryFilter(d => !d.IsDeleted);

            modelBuilder.Entity<Device>()
                .HasOne(d => d.Employee)
                .WithMany(e => e.Devices)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Device>()
                .HasOne(d => d.Category)
                .WithMany(c=> c.Devices)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Device>().HasData(
                new Device { Id = 1, Name = "Laptop1", Status = DeviceStatus.Active, IsDeleted = false, CategoryId = 1, EmployeeId = 1, 
                    CreatedAt = new DateTime(2026,1,1,0,0,0, DateTimeKind.Utc), UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Device { Id = 2, Name = "Desktop1", Status = DeviceStatus.Inactive, IsDeleted = false, CategoryId = 2, EmployeeId = 2,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Device { Id = 3, Name = "Monitor1", Status = DeviceStatus.Retired, IsDeleted = false, CategoryId = 3, EmployeeId = 1,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
                 );

            modelBuilder.Entity<User>().HasData(
                new User  {Id = 1, Username = "admin", PasswordHash = "$2a$11$ogfduxgaoWgAXcmymvqCG.Xd3ud9PrXV.0eFiCMDuu2Kx25cnf/pu",  Role = "Admin" },
                new User { Id = 2, Username = "JasonYan", PasswordHash = "$2a$11$ogfduxgaoWgAXcmymvqCG.Xd3ud9PrXV.0eFiCMDuu2Kx25cnf/pu", Role = "User"}
                );
            
        }

    }
}
