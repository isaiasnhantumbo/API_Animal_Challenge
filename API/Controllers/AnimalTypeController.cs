using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.AnimalTypes;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AnimalTypeController : BaseController
    {
        [HttpGet]
        public async Task<IReadOnlyList<AnimalType>> ListAnimalType()
        {
            return await Mediator.Send(new ListAnimalType.ListAnimalTypeQuery());
        }

        [HttpGet("{description}")]
        public async Task<ActionResult<AnimalType>> GetAnimalTypeByDescription(string description)
        {
            return await Mediator.Send(new GetAnimalTypeByDescription.GetAnimalTypeByDescriptionQuery
                {Description = description});
        }

        [HttpPost]
        public async Task<ActionResult<AnimalType>> AddAnimalType(AddAnimalType.AddAnimalTypeCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalType>> UpdateAnimalType(int id, UpdateAnimalType.UpdateAnimalTypeCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnimalType>> RemoveAnimalType(int id)
        {
            return await Mediator.Send(new DeleteAnimalType.DeleteAnimalTypeCommand {Id = id});
        }
    }
}