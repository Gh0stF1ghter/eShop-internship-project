using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using Baskets.UnitTests.BasketFakeDb;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baskets.UnitTests.Mock
{
    internal class UnitOfWorkMock : Mock<IUnitOfWork>
    {
        public UnitOfWorkMock GetBasketByIdAsync()
        {
            Setup(unitOfWork => unitOfWork.Basket.GetByConditionAsync(It.IsAny<Expression<Func<UserBasket, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FakeDb.Baskets.First());

            return this;
        }
    }
}
