using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record DeleteVendorComand(int Id, bool TrackChanges) : IRequest;
}
