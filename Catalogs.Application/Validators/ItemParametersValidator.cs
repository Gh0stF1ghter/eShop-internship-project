using Catalogs.Domain.RequestFeatures;
using FluentValidation;

namespace Catalogs.Application.Validators
{
    public class ItemParametersValidator : AbstractValidator<ItemParameters>
    {
        public ItemParametersValidator()
        {
            RuleFor(ip => ip.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber cannot be null or negative");
            RuleFor(ip => ip.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize cannot be null or negative");

            RuleFor(ip => ip.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("MinPrice must not be negative");
            RuleFor(ip => ip.MaxPrice)
                .GreaterThanOrEqualTo(ip => ip.MinPrice).WithMessage("MaxPrice must not be less than MinPrice");

        }
    }
}
