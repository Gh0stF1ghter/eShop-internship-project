using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record DeleteItemCommand(int TypeId, int Id, bool TrackChanges) : IRequest;
}
