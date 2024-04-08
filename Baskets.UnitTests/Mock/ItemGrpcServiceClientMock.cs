using Grpc.Core;
using ItemGrpcService;

namespace Baskets.UnitTests.Mock
{
    internal class ItemGrpcServiceClientMock : Mock<ItemService.ItemServiceClient>
    {
        public void GetItem(GetItemResponse itemResponse)
        {
            Setup(client => client.GetItemAsync(It.IsAny<GetItemRequest>(), null, null, It.IsAny<CancellationToken>()))
                .Returns(new AsyncUnaryCall<GetItemResponse>(
                    Task.FromResult(itemResponse), // This is the response
                    Task.FromResult(new Metadata()), // This is the response headers
                    () => Status.DefaultSuccess, // This is the status
                    () => [], // This is the trailers
                    () => { } // This is the dispose action
                ));
        }
    }
}
