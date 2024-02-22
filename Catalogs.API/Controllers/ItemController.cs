using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using Catalogs.Domain.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/types/{typeId}/items")]
    public class ItemController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpGet(Name = "GetItemsOfType")]
        [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllItemsOfTypeAsync(int typeId, [FromQuery] ItemParameters itemParameters, CancellationToken token = default)
        {
            var pagedResult = await _sender.Send(new GetItemsOfTypeQuery(typeId, itemParameters, TrackChanges: false), token);

            Response.Headers.Append("Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.items);
        }

        [HttpGet("{id}", Name = "GetItemOfType")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemOfTypeByIdAsync(int typeId, int id, CancellationToken token = default)
        {
            var item = await _sender.Send(new GetItemOfTypeQuery(id, typeId, TrackChanges: false), token);

            return Ok(item);
        }

        [HttpPost(Name = "AddItemOfType")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemAsync(int typeId, [FromBody] ItemManipulateDto item, CancellationToken token = default)
        {
            var newItem = await _sender.Send(new CreateItemCommand(item, typeId, TrackChanges: false), token);

            return CreatedAtRoute("GetItemOfType", new { typeId, id = newItem.Id }, newItem);
        }

        [HttpPut("{id}", Name = "UpdateItemOfType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemOfTypeAsync(int typeId, int id, [FromBody] ItemManipulateDto item, CancellationToken token = default)
        {
            await _sender.Send(new UpdateItemCommand(typeId, id, item, TrackChanges: true), token);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteItemOfType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteItemOfTypeAsync(int typeId, int id, CancellationToken token = default)
        {
            await _sender.Send(new DeleteItemCommand(typeId, id, TrackChanges: false), token);

            return NoContent();
        }
    }
}
