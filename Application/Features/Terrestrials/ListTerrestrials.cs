using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Terrestrials
{
    public class ListTerrestrials
    {
        public class ListTerrestrialsQuery : IRequest<IReadOnlyList<Terrestrial>>

        {
        }

        public class ListTerrestrialsHandler:IRequestHandler<ListTerrestrialsQuery,IReadOnlyList<Terrestrial>>
        {
            private readonly DataContext _context;
            public ListTerrestrialsHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<IReadOnlyList<Terrestrial>> Handle(ListTerrestrialsQuery request, CancellationToken cancellationToken)
            {
                return await _context.Terrestrials.Include(x => x.AnimalType).ToListAsync();
            }
        }
    }
}