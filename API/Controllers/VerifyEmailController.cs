using System.Threading.Tasks;
using Application.Dtos;
using Application.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class VerifyEmailController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> VerifyUserEmail(string key)
        {
            return await Mediator.Send(new VerifyUserEmail.VerifyUserEmailQuery(){Key = key});
        }
    }
}