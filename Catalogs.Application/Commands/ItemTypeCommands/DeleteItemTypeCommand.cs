using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record DeleteItemTypeComand(int Id, bool TrackChanges) : IRequest;
}
