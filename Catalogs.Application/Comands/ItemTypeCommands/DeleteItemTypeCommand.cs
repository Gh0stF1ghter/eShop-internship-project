using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record DeleteItemTypeComand(int Id, bool TrackChanges) : IRequest;
}
