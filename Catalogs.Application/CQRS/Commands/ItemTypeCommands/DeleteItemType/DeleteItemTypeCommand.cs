using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemTypeCommands.DeleteItemType
{
    public sealed record DeleteItemTypeCommand(int Id, bool TrackChanges) : IRequest;
}
