using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Birds
{
    public class ListBirds
    {
        public class ListBirdsQuery : IRequest<IReadOnlyList<Bird>>
        {
        }
        
        public class ListBirdsHandler : IRequestHandler<ListBirdsQuery, IReadOnlyList<Bird>>
        {
            private readonly DataContext _context;

            public ListBirdsHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<IReadOnlyList<Bird>> Handle(ListBirdsQuery request, CancellationToken cancellationToken)
            {
                return await _context.Birds
                    .Include(x=>x.AnimalType) // Adicionar o incluide para retorna o tipo de animal   
                    .ToListAsync();
            }
        }
    }
}