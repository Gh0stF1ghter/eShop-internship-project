using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record DeleteBrandComand(int Id, bool TrackChanges) : IRequest;
}
