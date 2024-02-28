using Catalogs.Application.DataTransferObjects.CreateDTOs;
using FluentValidation;

namespace Catalogs.Application.Validators
{
    public class BrandValidator : AbstractValidator<BrandManipulateDto>
    {
        public BrandValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Brand Name must not be empty")
                .MinimumLength(2).WithMessage("Brand Name is shorter than 2 characters")
                .MaximumLength(20).WithMessage("Brand Name is longer than 20 characters");
        }
    }
}
