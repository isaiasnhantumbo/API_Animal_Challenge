using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.TerrestrialTypes;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TerrestrialTypeController : BaseController
    {
        [HttpGet]
        public async Task<IReadOnlyList<TerrestrialType>> ListTerrestrialType()
        {
            return await Mediator.Send(new ListTerrestrialTypes.ListTerrestrialTypesQuery());
        }

        [HttpPost]
        public async Task<ActionResult<TerrestrialType>> AddTerrestrialType(
            AddTerrestrialTypes.AddTerrestrialTypesCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TerrestrialType>> RemoveTerrestrialType(int id)
        {
            return await Mediator.Send(new RemoveTerrestrialTypes.RemoveTerrestrialTypesCommand {Id = id});
        }
    }
    
    
}