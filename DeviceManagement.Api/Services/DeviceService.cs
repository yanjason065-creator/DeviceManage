using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeviceManagement.Api.Data;
using DeviceManagement.Api.DTOs;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Models.Common;
using DeviceManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DeviceManagement.Api.Services
{
    public class DeviceService:IDeviceService
    {

        //private List<Device> _devices;

        private readonly ILogger<DeviceService> _logger;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper; 

        public DeviceService(AppDbContext context, 
            ILogger<DeviceService> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper;
        }

        public DeviceDto AddDevice(CreateDeviceDto dto)
        {

            var exists = 
                _context.Devices
                .Any(x=>x.Name == dto.Name &&
                !x.IsDeleted);
            if (exists)
            {
                throw new ConflictException(ErrorMessages.DeviceAlreadyExists);
            }

            var entity = new Device 
            { 
                Name = dto.Name, 
                Status = dto.Status, 
                EmployeeId = dto.EmployeeId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            //Check if EmployeeId exists or not
            var employee = _context.Employees
                .FirstOrDefault(x=>x.Id==dto.EmployeeId);
            if(employee is null)
            {
                throw new NotFoundException(ErrorMessages.EmployeeNotFound);
            }

            var category = _context.Categories
                .FirstOrDefault(x=>x.Id==dto.CategoryId);
            if (category is null)
            {
                throw new NotFoundException(ErrorMessages.CategoryNotFound);
            }

            _context.Devices.Add(entity);
            _context.SaveChanges();

            var device = _context.Devices
                .Include(d => d.Category)
                .Include(d => d.Employee)
                .First(d => d.Id == entity.Id);

            return _mapper.Map<DeviceDto>(device);            
            
        }

        public Device Create(Device device)
        {

            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDevice(long id)
        {
            var device = _context.Devices.FirstOrDefault(d=>d.Id == id);
            if (device == null)
            {
                return false;
            }
            if(device.IsDeleted)
            {
                throw new NotFoundException(ErrorMessages.DeviceAlreadyDeleted);
            }
            //_context.Devices.Remove(device);
            //Soft Delete
            device.IsDeleted = true;
            device.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return true;
        }

        public Device GetById(long id)
        {
            throw new NotImplementedException();
        }

        private static IQueryable<Device> ApplySorting(
            IQueryable<Device> query,
            DeviceQuery request)
            {
                return request.SortBy?.ToLower() switch
                {
                    "name" => request.IsDescending
                        ? query.OrderByDescending(x => x.Name)
                        : query.OrderBy(x => x.Name),

                    "status" => request.IsDescending
                        ? query.OrderByDescending(x => x.Status)
                        : query.OrderBy(x => x.Status),

                    _ => query.OrderBy(x => x.Id)
                };
            }

        public PagedResults<DeviceDto> GetDevices(DeviceQuery query)
        {
                      
            _logger.LogInformation("Getting devices with query: {@query}", query);

            var result = _context.Devices.AsNoTracking().AsQueryable();
            if (query.IsDeleted == true)
            {
                result = _context.Devices
                    .IgnoreQueryFilters()
                    .Where(d => d.IsDeleted)
                    .AsNoTracking();
            }
            else
            {
                result = _context.Devices.AsNoTracking();
            }

            if (!string.IsNullOrEmpty(query.Name)) 
            {
                result = result.Where(d => d.Name.Contains(query.Name));
            }
            if (query.Status.HasValue) {
                result = result.Where(d=>d.Status == query.Status.Value);
            }
            

                //result = result.OrderBy(d => d.Name);
                var TotalCount = result.Count();
            result = ApplySorting(result, query);

            return new PagedResults<DeviceDto>
            {
                              
                //Items = result.Include(d=>d.Category)
                //.Include(d=>d.Employee)
                //.Skip((query.Page - 1) * query.PageSize)
                //.Take(query.PageSize)
                //.Select(d => new DeviceDto
                //{
                //    Id = d.Id,
                //    Name = d.Name,
                //    Status = d.Status.ToString(),
                //    DeleteStatus = d.IsDeleted,
                //    CategoryName = d.Category.Name,
                //    UserName = d.Employee.Name
                //}).ToList(),


                Items = result
                .Include(d => d.Category)
                .Include(d => d.Employee)
                .ProjectTo<DeviceDto>(_mapper.ConfigurationProvider)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList(),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = TotalCount
            };
   
        }

        public bool Update(Device device)
        {
            throw new NotImplementedException();
        }

        public DeviceDto UpdateDevice(long id, UpdateDeviceDto dto)
        {
            _logger.LogInformation("UpdateDevice started. Id: {Id}", id);
            var device = _context.Devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
            {
                _logger.LogWarning("Device not found. Id: {Id}", id);
               throw new NotFoundException(ErrorMessages.DeviceNotFound); ;
            }
            //Check Name Dupicate
            var exists =
               _context.Devices
               .Any(x => x.Name == dto.Name &&
               x.Id != id && //Most important
               !x.IsDeleted);
            if (exists)
            {
                throw new ConflictException(ErrorMessages.DeviceAlreadyExists);
            }

            //Check employee exists or not
            var employeeExists =
                _context.Employees.Any(e =>
                e.Id == dto.EmployeeId);

            if (!employeeExists)
            {
                throw new NotFoundException(
                    ErrorMessages.EmployeeNotFound);
            }


            var categoryExists =
                _context.Categories.Any(c =>
                c.Id == dto.CategoryId);

            if (!categoryExists)
            {
                throw new NotFoundException(
                    ErrorMessages.CategoryNotFound);
            }


            device.Name = dto.Name;
            device.Status = dto.Status;
            device.IsDeleted = dto.IsDeleted;
            device.EmployeeId = dto.EmployeeId;
            device.CategoryId = dto.CategoryId;

            device.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();


            _logger.LogInformation("Device updated successfully. Id: {Id}", id);

            device = _context.Devices
                .Include(d => d.Category)
                .Include(d => d.Employee)
                .First(d => d.Id == device.Id);

            return _mapper.Map<DeviceDto>(device);

            //return new DeviceDto { Id = device.Id, Name = device.Name,Status = device.Status.ToString()};
        }
    }
}
