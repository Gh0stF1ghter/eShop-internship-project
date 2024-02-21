using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
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
                .NotEmpty().WithMessage("Item Price must not be empty")
                .GreaterThan(0).WithMessage("Item Price must not equal zero or negative number");

            RuleFor(i => i.Stock)
                .NotNull().WithMessage("Item Stock must not be null")
                .GreaterThanOrEqualTo(0).WithMessage("Item Stock must not be negative");

            RuleFor(i => i.ImageUrl)
                .Must(IsValidUri).When(i => !string.IsNullOrEmpty(i.ImageUrl));
        }

        private bool IsValidUri(string uri) =>
            Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
    }
}
