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
    public class UpdateAnimalType
    {
        public class UpdateAnimalTypeCommand : IRequest<AnimalType>
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }
        
        public class UpdateAnimalTypeValidator : AbstractValidator<UpdateAnimalTypeCommand>
        {
            public UpdateAnimalTypeValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
            }
        }
        
        public class UpdateAnimalTypeHandler : IRequestHandler<UpdateAnimalTypeCommand, AnimalType>
        {
            private readonly DataContext _context;

            public UpdateAnimalTypeHandler(DataContext context)
            {
                _context = context;
            }
            
            public async Task<AnimalType> Handle(UpdateAnimalTypeCommand request, CancellationToken cancellationToken)
            {
                var animalType = await _context.AnimalTypes.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == request.Id);
                if (animalType is null)
                {
                    throw new Exception("Animal type not found");
                }

                var animalTypeCheck = await _context.AnimalTypes
                    .Where(x => x.Description.ToUpper() == request.Description.ToUpper() && x.Id != animalType.Id)
                    .FirstOrDefaultAsync();
                if (animalTypeCheck is not null)
                {
                    throw new Exception("Animal type exist in database");
                }

                animalType.Description = request.Description;
                _context.Entry(animalType).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Fail to update the animal type");
                }

                return animalType;
            }
        }
    }
}