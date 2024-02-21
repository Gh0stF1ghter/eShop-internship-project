using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogs.API.Controllers
{
    [ApiController]
    [Route("api/vendors")]
    public class VendorController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpGet(Name = "GetVendors")]
        [ProducesResponseType(typeof(IEnumerable<VendorDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllVendorsAsync(CancellationToken token)
        {
            var vendors = await _sender.Send(new GetVendorsQuery(TrackChanges: false), token);

            return Ok(vendors);
        }

        [HttpGet("{id}", Name = "GetVendorById")]
        [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVendorByIdAsync(int id, CancellationToken token)
        {
            var vendor = await _sender.Send(new GetVendorQuery(id, TrackChanges: false), token);

            return Ok(vendor);
        }

        [HttpPost(Name = "AddVendor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddVendorAsync(VendorManipulateDto vendor, CancellationToken token)
        {
            var newVendor = await _sender.Send(new CreateVendorCommand(vendor), token);

            return CreatedAtAction("GetVendorById", new { newVendor.Id }, newVendor);
        }

        [HttpPut("{id}", Name = "UpdateVendor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVendorAsync(int id, VendorManipulateDto vendor, CancellationToken token)
        {
            await _sender.Send(new UpdateVendorComand(id, vendor, TrackChanges: true), token);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteVendor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVendorAsync(int id, CancellationToken token)
        {
            await _sender.Send(new DeleteVendorComand(id, TrackChanges: false), token);

            return NoContent();
        }
    }
}
