using AutoMapper;
using ItemGrpcService;
using MediatR;

namespace Catalogs.API.GrpcServices
{
    public class ItemGrpcService(ISender sender, IMapper mapper, ILogger logger) : ItemService.ItemServiceBase
    {
        public override async Task<GetItemResponse> GetItem(GetItemRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
