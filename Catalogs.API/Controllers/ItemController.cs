using Catalogs.API.ActionFilters;
using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;

namespace Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/types/{typeId}/items")]
    public class ItemController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        [ActionName("GetItemsOfType")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ProducesResponseType(typeof(IEnumerable<ExpandoObject>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllItemsOfTypeAsync(int typeId, [FromQuery] ItemParameters itemParameters, CancellationToken token = default)
        {
            var linkParams = new LinkParameters(itemParameters, HttpContext);

            var (linkResponse, metaData) = await _sender.Send(new GetItemsOfTypeQuery(typeId, linkParams, TrackChanges: false), token);

            Response.Headers.Append("Pagination", JsonSerializer.Serialize(metaData));

            return linkResponse.HasLinks ? Ok(linkResponse.LinkedEntities) : Ok(linkResponse.ShapedEntities);
        }

        [HttpGet("{id}")]
        [ActionName("GetItemOfType")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemOfTypeByIdAsync(int typeId, int id, CancellationToken token = default)
        {
            var item = await _sender.Send(new GetItemOfTypeQuery(id, typeId, TrackChanges: false), token);

            return Ok(item);
        }

        [HttpPost]
        [ActionName("AddItemOfType")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItemAsync(int typeId, [FromBody] ItemManipulateDto item, CancellationToken token = default)
        {
            var newItem = await _sender.Send(new CreateItemComand(item, typeId, TrackChanges: false), token);

            return CreatedAtRoute("GetItemOfType", new { typeId, id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        [ActionName("UpdateItemOfType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemOfTypeAsync(int typeId, int id, [FromBody] ItemManipulateDto item, CancellationToken token = default)
        {
            await _sender.Send(new UpdateItemComand(typeId, id, item, TrackChanges: true), token);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteItemOfType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteItemOfTypeAsync(int typeId, int id, CancellationToken token = default)
        {
            await _sender.Send(new DeleteItemComand(typeId, id, TrackChanges: false), token);

            return NoContent();
        }
    }
}
