using Catalogs.Application.Queries.ItemQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpGet(Name = "GetItems")]
        public async Task<IActionResult> GetItemsAsync(CancellationToken token = default)
        {
            var items = await _sender.Send(new GetItemsQuery(TrackChanges: false), token);

            return Ok(items);
        }
    }
}
