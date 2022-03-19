using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.Birds;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BirdController : BaseController
    {
        [HttpGet]
        public async Task<IReadOnlyList<Bird>> ListBird()
        {
            return await Mediator.Send(new ListBirds.ListBirdsQuery());
        }

        [HttpPost("{animalTypeId}")]
        public async Task<ActionResult<Bird>> AddBird(int animalTypeId, AddBird.AddBirdCommand command)
        {
            command.AnimalTypeId = animalTypeId;
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Bird>> UpdateBird(int id, UpdateBird.UpdateBirdCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Bird>> DeleteBird(int id)
        {
            return await Mediator.Send(new DeleteBird.DeleteBirdCommand {Id = id});
        }
    }
}