using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Birds
{
    public class DeleteBird
    {
        public class DeleteBirdCommand : IRequest<Bird>
        {
            public int Id { get; set; }
        }
        
        public class DeleteBirdHandler : IRequestHandler<DeleteBirdCommand, Bird>
        {
            private readonly DataContext _context;

            public DeleteBirdHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Bird> Handle(DeleteBirdCommand request, CancellationToken cancellationToken)
            {
                var bird = await _context.Birds
                    .Include(x => x.AnimalType)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                
                if (bird is null)
                {
                    throw new Exception("Bird not found");
                }

                _context.Birds.Remove(bird);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Fail to delete bird");
                }

                return bird;
            }
        }
    }
}