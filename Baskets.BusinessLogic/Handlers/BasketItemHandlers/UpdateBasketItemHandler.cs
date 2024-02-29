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
    public class UpdateBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBasketItemComand>
    {
        public Task Handle(UpdateBasketItemComand comand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
