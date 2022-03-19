using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.TerrestrialTypes
{
    public class ListTerrestrialTypes
    {
        public class ListTerrestrialTypesQuery : IRequest<IReadOnlyList<TerrestrialType>>
        {
        }
        
        public class ListTerrestrialTypesHandler : IRequestHandler<ListTerrestrialTypesQuery, 
            IReadOnlyList<TerrestrialType>>
        {
            private readonly DataContext _context;

            public ListTerrestrialTypesHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<IReadOnlyList<TerrestrialType>> Handle(ListTerrestrialTypesQuery request, CancellationToken cancellationToken)
            {
                return await _context.TerrestrialTypes.ToListAsync();
            }
        }
    }
}