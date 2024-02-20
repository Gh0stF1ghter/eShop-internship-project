using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record DeleteItemComand(int Id, bool TrackChanges) : IRequest;
}
