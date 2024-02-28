using MediatR;

namespace Catalogs.Application.Comands.ItemCommands
{
    public sealed record DeleteItemComand(int TypeId, int Id, bool TrackChanges) : IRequest;
}
