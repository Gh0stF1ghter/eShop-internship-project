using Identity.BusinessLogic.Services.Interfaces;
using Identity.DataAccess.Data;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.EventBus;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await service.GetAllUsersAsync(cancellationToken);

            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await service.DeleteUserAsync(id, cancellationToken);

            return NoContent();
        }
    }
}