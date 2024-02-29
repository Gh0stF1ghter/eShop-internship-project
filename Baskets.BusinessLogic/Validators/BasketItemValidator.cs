using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
