using MediatR;

namespace Catalogs.Application.CQRS.Commands.BrandCommands.DeleteBrand
{
    public sealed record DeleteBrandCommand(int Id, bool TrackChanges) : IRequest;
}
