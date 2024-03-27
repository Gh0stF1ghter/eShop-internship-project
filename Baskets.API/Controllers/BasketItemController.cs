using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.CreateBasketItem;
using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.DeleteBasketItem;
using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.UpdateBasketItem;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItem;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItems;
using Baskets.BusinessLogic.DataTransferObjects.CreateDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Baskets.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/basket/items")]
    public class BasketItemController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [ActionName("GetBasketItems")]
        public async Task<IActionResult> GetBasketItemsAsync([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var basketItems = await sender.Send(new GetBasketItemsQuery(userId), cancellationToken);

            return Ok(basketItems);
        }

        [HttpGet("{basketItemId}")]
        [ActionName("GetBasketItemById")]
        public async Task<IActionResult> GetBasketItemByIdAsync([FromRoute] string userId, [FromRoute] string basketItemId, CancellationToken cancellationToken)
        {
            var basketItem = await sender.Send(new GetBasketItemQuery(userId, basketItemId), cancellationToken);

            return Ok(basketItem);
        }

        [HttpPost]
        [ActionName("CreateBasketItem")]
        public async Task<IActionResult> CreateBasketItemAsync([FromRoute] string userId, [FromBody] CreateBasketItemDto createBasketItemDto, CancellationToken cancellationToken)
        {
            var basketItem = await sender.Send(new CreateBasketItemCommand(userId, createBasketItemDto), cancellationToken);

            return CreatedAtAction("GetBasketItemById", new { userId, basketItemId = basketItem.BasketItemId }, basketItem);
        }

        [HttpPut("{basketItemId}")]
        [ActionName("UpdateBasketItem")]
        public async Task<IActionResult> UpdateBasketItemAsync([FromRoute] string userId, [FromRoute] string basketItemId, [FromQuery] uint quantity, CancellationToken cancellationToken)
        {
            await sender.Send(new UpdateBasketItemCommand(userId, basketItemId, (int)quantity), cancellationToken);

            return NoContent();
        }

        [HttpDelete("{basketItemId}")]
        [ActionName("DeleteBasketItem")]
        public async Task<IActionResult> DeleteBasketItemAsync([FromRoute] string userId, [FromRoute] string basketItemId, CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteBasketItemCommand(userId, basketItemId), cancellationToken);

            return NoContent();
        }
    }
}