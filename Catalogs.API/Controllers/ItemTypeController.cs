using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/itemTypes")]
    public class ItemTypeController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpGet(Name = "GetItemTypes")]
        [ProducesResponseType(typeof(IEnumerable<ItemTypeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllItemTypesAsync(CancellationToken token)
        {
            var itemTypes = await _sender.Send(new GetItemTypesQuery(TrackChanges: false), token);

            return Ok(itemTypes);
        }

        [HttpGet("{id}", Name = "GetItemTypeById")]
        [ProducesResponseType(typeof(ItemTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemTypeByIdAsync(int id, CancellationToken token)
        {
            var itemType = await _sender.Send(new GetItemTypeQuery(id, TrackChanges: false), token);

            return Ok(itemType);
        }

        [HttpPost(Name = "AddItemType")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemTypeAsync(ItemTypeManipulateDto itemType, CancellationToken token)
        {
            var newItemType = await _sender.Send(new CreateItemTypeCommand(itemType), token);

            return CreatedAtAction("GetItemTypeById", new { newItemType.Id }, newItemType);
        }

        [HttpPut("{id}", Name = "UpdateItemType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemTypeAsync(int id, ItemTypeManipulateDto itemType, CancellationToken token)
        {
            await _sender.Send(new UpdateItemTypeCommand(id, itemType, TrackChanges: true), token);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteItemType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteItemTypeAsync(int id, CancellationToken token)
        {
            await _sender.Send(new DeleteItemTypeComand(id, TrackChanges: false), token);

            return NoContent();
        }
    }
}
