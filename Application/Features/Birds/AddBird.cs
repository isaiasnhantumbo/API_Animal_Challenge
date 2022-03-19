using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Features.Birds
{
    public class AddBird
    {
        public class AddBirdCommand : IRequest<Bird>
        {
            public string Name { get; set; }
            public string Color { get; set; }
            public int AnimalTypeId { get; set; }
        }
        
        public class AddBirdValidator : AbstractValidator<AddBirdCommand>
        {
            public AddBirdValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Color).NotEmpty();
            }
        }
        
        public class AddBirdHandler : IRequestHandler<AddBirdCommand, Bird>
        {
            private readonly DataContext _context;

            public AddBirdHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<Bird> Handle(AddBirdCommand request, CancellationToken cancellationToken)
            {
                var animalType = await _context.AnimalTypes.FindAsync(request.AnimalTypeId);
                if (animalType is null || animalType.Description != "Bird")
                {
                    throw new Exception("Animal Type not found or is not Bird");
                }

                var bird = new Bird
                {
                    Name = request.Name,
                    Color = request.Color,
                    AnimalType = animalType
                };

                await _context.Birds.AddAsync(bird);
                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new Exception("Fail to add bird");
                }

                return bird;
            }
        }
    }
}