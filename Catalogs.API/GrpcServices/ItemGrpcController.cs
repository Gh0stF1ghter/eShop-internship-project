using AutoMapper;
using Catalogs.Application.Queries.ItemQueries;
using Grpc.Core;
using ItemGrpcService;
using MediatR;

namespace Catalogs.API.GrpcServices
{
    public class ItemGrpcController(ISender sender, IMapper mapper, ILogger<ItemGrpcController> logger) : ItemService.ItemServiceBase
    {
        public override async Task<GetItemResponse> GetItem(GetItemRequest request, ServerCallContext serverCallContext)
        {
            logger.LogInformation("getting item {id} of type from query", request.Id);
            var itemDto = await sender.Send(new GetItemQuery(request.Id));

            var item = mapper.Map<Item>(itemDto);

            var itemResponse = new GetItemResponse
            {
                Item = item,
            };

            logger.LogInformation("sending item {item}", item);
            return itemResponse;
        }
    }
}