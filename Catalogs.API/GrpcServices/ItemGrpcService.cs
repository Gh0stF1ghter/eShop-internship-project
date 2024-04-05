using AutoMapper;
using Catalogs.Application.Queries.ItemQueries;
using Grpc.Core;
using ItemGrpcService;
using MediatR;

namespace Catalogs.API.GrpcServices
{
    public class ItemGrpcController(ISender sender, IMapper mapper, ILogger logger) : ItemService.ItemServiceBase
    {
        public override async Task<GetItemResponse> GetItem(GetItemRequest request, ServerCallContext serverCallContext)
        {
            logger.LogInformation("getting item {id} of type {typeId} from query", request.Id, request.TypeId);
            var itemDto = await sender.Send(new GetItemOfTypeQuery(request.Id, request.TypeId, TrackChanges: false));

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