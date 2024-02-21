using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpGet(Name = "GetBrands")]
        [ProducesResponseType(typeof(IEnumerable<BrandDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrandsAsync(CancellationToken token)
        {
            var brands = await _sender.Send(new GetBrandsQuery(TrackChanges: false), token);

            return Ok(brands);
        }

        [HttpGet("{id}", Name = "GetBrandById")]
        [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBrandByIdAsync(int id, CancellationToken token)
        {
            var brand = await _sender.Send(new GetBrandQuery(id, TrackChanges: false), token);

            return Ok(brand);
        }

        [HttpPost(Name = "AddBrand")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBrandAsync(BrandManipulateDto brand, CancellationToken token)
        {
            var newBrand = await _sender.Send(new CreateBrandCommand(brand), token);

            return CreatedAtAction("GetBrandById", new { newBrand.Id }, newBrand);
        }

        [HttpPut(Name = "UpdateBrand")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBrandAsync(int id, BrandManipulateDto brand, CancellationToken token)
        {
            await _sender.Send(new UpdateBrandCommand(id, brand, TrackChanges: true), token);

            return NoContent();
        }

        [HttpDelete(Name = "DeleteBrand")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBrandAsync(int id, CancellationToken token)
        {
            await _sender.Send(new DeleteBrandComand(id, TrackChanges: false), token);

            return NoContent();
        }
    }
}
