using DeviceManagement.Api.DTOs;
using FluentValidation;

namespace DeviceManagement.Api.Validators
{
    public class CreateDeviceDtoValidator : AbstractValidator<CreateDeviceDto>
    {
        public CreateDeviceDtoValidator() {
            RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Device name is required.")
                    .Length(2, 50)
                    .WithMessage("Device name must be between 2 and 50 characters.");

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);
        }
    }
    
    
}
