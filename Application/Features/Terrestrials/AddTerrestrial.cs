using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain;
using FluentValidation;
using Persistence;

namespace Application.Features.Terrestrials
{
    public class AddTerrestrial
    {
        public class AddTerrestrialCommand : IRequest<Terrestrial>
        {
            public string Name { get; set; }
            public string Color { set; get; }
            public int LegsNumber { get; set; }
            public bool IsFur { set; get; }
            public int TerrestrialTypeId { get; set; }
            public int AnimalTypeId { get; set; }
        }

        public class AddTerrestrialValidator : AbstractValidator<AddTerrestrialCommand>
        {
            public AddTerrestrialValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Color).NotEmpty();
                // RuleFor(x => x.IsFur).NotEmpty();
                RuleFor(x => x.LegsNumber).NotEmpty();
            }
        }

        public class AddTerrestrialHandler : IRequestHandler<AddTerrestrialCommand, Terrestrial>
        {
            private readonly DataContext _context;

            public AddTerrestrialHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Terrestrial> Handle(AddTerrestrialCommand request, CancellationToken cancellationToken)
            {
                var terrestrialType = await _context.TerrestrialTypes.FindAsync(request.TerrestrialTypeId);
                var animalType = await _context.AnimalTypes.FindAsync(request.AnimalTypeId);

                if (animalType is null || animalType.Description != "Terrestrial")
                {
                    throw new Exception("Animal Type not found or is not Terrestrial");
                }

                if (terrestrialType is null)
                {
                    throw new Exception("Terrestrial Type not found");
                }

                var terrestrial = new Terrestrial
                {
                    Name = request.Name,
                    Color = request.Color,
                    LegsNumber = request.LegsNumber,
                    IsFur = request.IsFur,
                    AnimalType = animalType,
                    TerrestrialType = terrestrialType
                };
                await _context.Terrestrials.AddAsync(terrestrial);
                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new Exception("Error to add Terrestrial");
                }

                return terrestrial;
            }
        }
    }
}