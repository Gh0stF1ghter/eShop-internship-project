using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record DeleteVendorComand(int Id, bool TrackChanges) : IRequest;
}
