using FluentValidation;

namespace Baskets.BusinessLogic.Validators
{
    public class BasketItemValidator : AbstractValidator<CreateBasketItemDto>
    {
        public BasketItemValidator()
        {
            RuleFor(bi => bi.ItemId)
                .NotEmpty().WithMessage("Item id cannot be empty");
        }
    }
}