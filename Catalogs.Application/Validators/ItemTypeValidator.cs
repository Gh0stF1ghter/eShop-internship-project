using Catalogs.Application.DataTransferObjects.CreateDTOs;
using FluentValidation;

namespace Catalogs.Application.Validators
{
    public class ItemTypeValidator : AbstractValidator<ItemTypeManipulateDto>
    {
        public ItemTypeValidator()
        {
            RuleFor(it => it.Name)
                .NotEmpty().WithMessage("Type Name must not be empty")
                .MinimumLength(2).WithMessage("Type Name is shorter than 2 characters")
                .MaximumLength(20).WithMessage("Type Name is longer than 20 characters");
        }
    }
}
