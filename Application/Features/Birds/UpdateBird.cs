using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Birds
{
    public class UpdateBird
    {
        public class UpdateBirdCommand : IRequest<Bird>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
        }
        
        public class UpdateBirdValidator : AbstractValidator<UpdateBirdCommand>
        {
            public UpdateBirdValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Color).NotEmpty();
            }
        }
        
        public class UpdateBirdHandler : IRequestHandler<UpdateBirdCommand, Bird>
        {
            private readonly DataContext _context;

            public UpdateBirdHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Bird> Handle(UpdateBirdCommand request, CancellationToken cancellationToken)
            {
                var bird = await _context.Birds.Include(x => x.AnimalType)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                if (bird is null)
                {
                    throw new Exception("Bird not found");
                }

                bird.Name = request.Name;
                bird.Color = request.Color;
                
                _context.Birds.Update(bird);
                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new Exception("Fail to update the bird");
                }

                return bird;
            }
        }
    }
}