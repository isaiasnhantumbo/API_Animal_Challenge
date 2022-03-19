using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Features.TerrestrialTypes
{
    public class RemoveTerrestrialTypes
    {
        public class RemoveTerrestrialTypesCommand : IRequest<TerrestrialType>
        {
            public int Id { get; set; }
        }

        public class RemoveTerrestrialTypesHandel : IRequestHandler<RemoveTerrestrialTypesCommand, TerrestrialType>
        {
            private readonly DataContext _context;

            public RemoveTerrestrialTypesHandel(DataContext context)
            {
                _context = context;
            }
            public async Task<TerrestrialType> Handle(RemoveTerrestrialTypesCommand request, CancellationToken cancellationToken)
            {
                var terrestrialType = await _context.TerrestrialTypes.FindAsync(request.Id);
                if (terrestrialType is null)
                {
                    throw new Exception("Terrestrial Type is not found");
                }

                _context.TerrestrialTypes.Remove(terrestrialType);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Fail to delete Terrestrial Type");
                }

                return terrestrialType;
            }
        }
    }
}