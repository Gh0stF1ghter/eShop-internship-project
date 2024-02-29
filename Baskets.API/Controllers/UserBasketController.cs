using Baskets.BusinessLogic.Comands.CustomerBasket;
using Baskets.BusinessLogic.Queries.CustomerBasket;
using MediatR;
using Microsoft.AspNetCore.Http;
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

            return CreatedAtAction("GetUserBasket", basket);
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
