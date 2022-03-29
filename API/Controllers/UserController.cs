using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUser.CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            return await Mediator.Send(new GetUser.GetUserQuery());
        }
    }
}