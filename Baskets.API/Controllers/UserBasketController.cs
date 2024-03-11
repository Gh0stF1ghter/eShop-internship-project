using Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.CreateUserBasketComand;
using Baskets.BusinessLogic.CQRS.Comands.UserBasketComands.DeleteUserBasketComand;
using Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasketQuery;
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

        [HttpPost]
        [ActionName("CreateUserBasket")]
        public async Task<IActionResult> CreateUserBasketAsync([FromRoute] string userId, CancellationToken cancelationToken)
        {
            var basket = await sender.Send(new CreateUserBasketComand(userId), cancelationToken);

            return CreatedAtAction("GetUserBasket", new { userId }, basket);
        }

        [HttpDelete]
        [ActionName("DeleteUserBasket")]
        public async Task<IActionResult> DeleteUserBasketAsync([FromRoute] string userId, CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteUserBasketComand(userId), cancellationToken);

            return NoContent();
        }
    }
}