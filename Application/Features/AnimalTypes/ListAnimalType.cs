using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class ListAnimalType
    {
        public class ListAnimalTypeQuery : IRequest<IReadOnlyList<AnimalType>>
        {
        }
        
        public class ListAnimalTypeHandler: IRequestHandler<ListAnimalTypeQuery, IReadOnlyList<AnimalType>>
        {
            private readonly DataContext _context;

            public ListAnimalTypeHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<IReadOnlyList<AnimalType>> Handle(ListAnimalTypeQuery request, CancellationToken cancellationToken)
            {
                return await _context.AnimalTypes.ToListAsync();
            }
        }
    }
}