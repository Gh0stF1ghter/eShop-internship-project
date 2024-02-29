using AutoMapper;
using Baskets.BusinessLogic.DataTransferObjects;
using Baskets.BusinessLogic.DataTransferObjects.CreateDTOs;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Baskets.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await unitOfWork.User.GetAllAsync(cancellationToken);

            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser(CancellationToken token, [FromBody] CreateUser createUser, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(createUser);

            unitOfWork.User.Add(user);

            return NoContent();
        }
    }
}
