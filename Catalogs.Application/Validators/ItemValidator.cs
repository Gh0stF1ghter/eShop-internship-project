using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using FluentValidation;

namespace Catalogs.Application.Validators
{
    public class ItemValidator : AbstractValidator<ItemManipulateDto>
    {
        public ItemValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty().WithMessage("Item Name must not be empty")
                .MinimumLength(2).WithMessage("Item is shorter than 2 characters")
                .MaximumLength(50).WithMessage("Item is longer than 20 characters");

            RuleFor(i => i.Price)
                .GreaterThan(0).WithMessage("Item Price must not equal zero or negative number");

            RuleFor(i => i.Stock)
                .NotNull().WithMessage("Item Stock must not be null")
                .GreaterThanOrEqualTo(0).WithMessage("Item Stock must not be negative");

            RuleFor(i => i.ImageUrl)
                .Must(IsValidUri).When(i => !string.IsNullOrEmpty(i.ImageUrl)).WithMessage("Image url is invalid");

            RuleFor(i => i.BrandId)
                .GreaterThan(0).WithMessage("Brand Id must be positive");

            RuleFor(i => i.VendorId)
                .GreaterThan(0).WithMessage("Vendor Id must be positive");
        }

        private bool IsValidUri(string uri)
        {
            var isValid = Uri.IsWellFormedUriString(uri, UriKind.Relative);

            return isValid;
        }
    }
}
