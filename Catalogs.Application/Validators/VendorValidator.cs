using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using FluentValidation;

namespace Catalogs.Application.Validators
{
    internal class VendorValidator : AbstractValidator<VendorManipulateDto>
    {
        public VendorValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Vendor Name must not be empty")
                .MinimumLength(2).WithMessage("Vendor is shorter than 2 characters")
                .MaximumLength(20).WithMessage("Vendor is longer than 20 characters");
        }
    }
}
