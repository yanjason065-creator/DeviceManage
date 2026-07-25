
using BCrypt.Net;
using DeviceManagement.Api.Data;
using DeviceManagement.Api.Middleware;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Services;
using DeviceManagement.Api.Services.Interfaces;
using DeviceManagement.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace DeviceManagement.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            string hash = BCrypt.Net.BCrypt.HashPassword("123456");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHealthChecks();

            builder.Host.UseSerilog();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
                    };
                });

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Enter JWT token: Bearer {your token}"
                    });

                options.AddSecurityRequirement(
                new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            //builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            if (builder.Environment.EnvironmentName == "Test")
            {
                builder.Services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(
                        "DataSource=:memory:");
                });
            }
            else
            {
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));
            }

            builder.Services.AddAutoMapper(typeof(Program));


            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            builder.Services.AddValidatorsFromAssemblyContaining<UpdateDeviceDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateDeviceDtoValidator>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                    .Where(x => x.Value?.Errors?.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value?.Errors?.Select(e => e.ErrorMessage)
                        .ToArray()??Array.Empty<string>());

                    var response = ApiResponse<object>.Fail("Validation failed");
                    response.Data = errors;

                    return new BadRequestObjectResult(response);
                };
            });

            


            var app = builder.Build();

            app.MapHealthChecks("/health");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

           

            app.UseHttpsRedirection();
           
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
