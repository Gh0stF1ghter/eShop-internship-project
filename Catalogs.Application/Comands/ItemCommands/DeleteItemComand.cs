using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record DeleteItemComand(int TypeId, int Id, bool TrackChanges) : IRequest;
}
