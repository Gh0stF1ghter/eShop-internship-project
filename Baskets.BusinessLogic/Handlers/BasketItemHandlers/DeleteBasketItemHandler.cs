using AutoMapper;
using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class DeleteBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteBasketItemComand>
    {
        public Task Handle(DeleteBasketItemComand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
