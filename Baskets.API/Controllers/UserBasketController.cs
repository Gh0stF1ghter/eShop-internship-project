using Baskets.API.Hubs;
using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket;
using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.DeleteUserBasket;
using Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasket;
using Baskets.DataAccess.Entities.Constants;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Baskets.API.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/basket")]
    public class UserBasketController(ISender sender) : ControllerBase
    {
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        [ActionName("GetUserBasket")]
        public async Task<IActionResult> GetUserBasketAsync([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var basket = await sender.Send(new GetUserBasketQuery(userId), cancellationToken);

            return Ok(basket);
        }
    }
}