using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Terrestrials
{
    public class DeleteTerrestrial
    {
        public class DeleteTerrestrialCommand:IRequest<Terrestrial>
        {
            public int Id { get; set; }
        }
        public class DeleteTerrestrialHandler:IRequestHandler<DeleteTerrestrialCommand,Terrestrial>
        {
            private readonly DataContext _context;

            public DeleteTerrestrialHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Terrestrial> Handle(DeleteTerrestrialCommand request, CancellationToken cancellationToken)
            {
                var terrestrial = await _context.Terrestrials
                    .Include(x => x.AnimalType)
                    .FirstOrDefaultAsync(x => x.Id== request.Id);
                if (terrestrial is null)
                {
                    throw new Exception("Terrestrial not found");
                }

                _context.Terrestrials.Remove(terrestrial);
                var result = await _context.SaveChangesAsync();

                if (result <=0)
                {
                    throw new Exception("Error to delete Terrestrial");
                }

                return terrestrial;

            }
        }
    }
}