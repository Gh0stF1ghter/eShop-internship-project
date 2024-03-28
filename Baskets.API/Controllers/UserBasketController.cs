using Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Baskets.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/basket")]
    public class UserBasketController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [ActionName("GetUserBasket")]
        public async Task<IActionResult> GetUserBasketAsync([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var basket = await sender.Send(new GetUserBasketQuery(userId), cancellationToken);

            return Ok(basket);
        }
    }
}