using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemCommands.DeleteItem
{
    public sealed record DeleteItemCommand(int TypeId, int Id, bool TrackChanges) : IRequest;
}
