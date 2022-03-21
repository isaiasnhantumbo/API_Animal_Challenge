using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.Terrestrials;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TerrestrialController : BaseController
    {
        [HttpGet]
        public async Task<IReadOnlyList<Terrestrial>> ListTerrestrial()
        {
            return await Mediator.Send(new ListTerrestrials.ListTerrestrialsQuery());
        }

        [HttpPost]
        public async Task<ActionResult<Terrestrial>> AddTerrestrial(AddTerrestrial.AddTerrestrialCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Terrestrial>> UpdateTerrestrial(int id,
            UpdateTerrestrial.UpdateTerrestrialCommand command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Terrestrial>> DeleteTerrestrial(int id)
        {
            return await Mediator.Send(new DeleteTerrestrial.DeleteTerrestrialCommand {Id = id});
        }

    }
}