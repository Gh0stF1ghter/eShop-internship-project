using MediatR;

namespace Catalogs.Application.Comands.ItemTypeCommands
{
    public sealed record DeleteItemTypeComand(int Id, bool TrackChanges) : IRequest;
}
