using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket;
using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.DeleteUserBasket;
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

        [HttpPost]
        [ActionName("CreateUserBasket")]
        public async Task<IActionResult> CreateUserBasketAsync([FromRoute] string userId, CancellationToken cancelationToken)
        {
            var basket = await sender.Send(new CreateUserBasketCommand(userId), cancelationToken);

            return CreatedAtAction("GetUserBasket", new { userId }, basket);
        }

        [HttpDelete]
        [ActionName("DeleteUserBasket")]
        public async Task<IActionResult> DeleteUserBasketAsync([FromRoute] string userId, CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteUserBasketCommand(userId), cancellationToken);

            return NoContent();
        }
    }
}