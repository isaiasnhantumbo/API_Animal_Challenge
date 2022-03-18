using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class AddAnimalType
    {
        public class AddAnimalTypeCommand : IRequest<AnimalType>
        {
            public string Description { get; set; }
        }
        
        public class AddAnimalTypeValidator : AbstractValidator<AddAnimalTypeCommand>
        {
            public AddAnimalTypeValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
            }
        }
        
        public class AddAnimalTypeHandler : IRequestHandler<AddAnimalTypeCommand, AnimalType>
        {
            private readonly DataContext _context;

            public AddAnimalTypeHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<AnimalType> Handle(AddAnimalTypeCommand request, CancellationToken cancellationToken)
            {
                var animalType = await _context.AnimalTypes
                    .Where(x => x.Description.ToUpper() == request.Description.ToUpper()).FirstOrDefaultAsync();
                if (animalType != null)
                {
                    throw new Exception("The animal type exist in database!");
                }

                animalType = new AnimalType
                {
                    Description = request.Description
                };

               await _context.AnimalTypes.AddAsync(animalType);
               var result = await _context.SaveChangesAsync();

               if (result <= 0)
               {
                   throw new Exception("Fail to add the animal type");
               }

               return animalType;
            }
        }
    }
}