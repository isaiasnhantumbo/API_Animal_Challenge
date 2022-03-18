using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Features.AnimalTypes
{
    public class DeleteAnimalType
    {
        public class DeleteAnimalTypeCommand : IRequest<AnimalType>
        {
            public int Id { get; set; }
        }
        
        public class DeleteAnimalTypeHandler : IRequestHandler<DeleteAnimalTypeCommand, AnimalType>
        {
            private readonly DataContext _context;

            public DeleteAnimalTypeHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<AnimalType> Handle(DeleteAnimalTypeCommand request, CancellationToken cancellationToken)
            {
                var animalType = await _context.AnimalTypes.FindAsync(request.Id);
                if (animalType is null)
                {
                    throw new Exception("Animal type not found");
                }

                _context.AnimalTypes.Remove(animalType);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Fail to delete the animal type");
                }

                return animalType;
            }
        }
    }
}