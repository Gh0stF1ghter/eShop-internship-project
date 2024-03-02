using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.BusinessLogic.DataTransferObjects.CreateDTOs;
using Baskets.BusinessLogic.Queries.BasketItem;
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

        [HttpGet("{id}")]
        [ActionName("GetBasketItemById")]
        public async Task<IActionResult> GetBasketItemByIdAsync([FromRoute] string userId, [FromRoute] string id, CancellationToken cancellationToken)
        {
            var basketItem = await sender.Send(new GetBasketItemQuery(userId, id), cancellationToken);

            return Ok(basketItem);
        }

        [HttpPost]
        [ActionName("CreateBasketItem")]
        public async Task<IActionResult> CreateBasketItemAsync([FromRoute] string userId, [FromBody] CreateBasketItemDto createBasketItemDto, CancellationToken cancellationToken)
        {
            var basketItem = await sender.Send(new CreateBasketItemComand(userId, createBasketItemDto), cancellationToken);

            return CreatedAtAction("GetBasketItemById", new { userId, basketItem.Id }, basketItem);
        }

        [HttpPut("{id}")]
        [ActionName("UpdateBasketItem")]
        public async Task<IActionResult> UpdateBasketItemAsync([FromRoute] string userId, [FromRoute] string id, [FromQuery] int quantity, CancellationToken cancellationToken)
        {
            await sender.Send(new UpdateBasketItemComand(userId, id, quantity), cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteBasketItem")]
        public async Task<IActionResult> DeleteBasketItemAsync([FromRoute] string userId, [FromRoute] string id, CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteBasketItemComand(userId, id), cancellationToken);

            return NoContent();
        }
    }
}
