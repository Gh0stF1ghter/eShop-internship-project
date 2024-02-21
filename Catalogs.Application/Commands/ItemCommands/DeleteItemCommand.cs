using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record DeleteItemCommand(int Id, bool TrackChanges) : IRequest;
}
