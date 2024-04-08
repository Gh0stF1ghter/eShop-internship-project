using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public record GetItemQuery(int Id) : IRequest<ItemDto>;
}
