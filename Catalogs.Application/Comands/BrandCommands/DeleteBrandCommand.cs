using MediatR;

namespace Catalogs.Application.Comands.BrandCommands
{
    public sealed record DeleteBrandComand(int Id, bool TrackChanges) : IRequest;
}
