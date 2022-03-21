using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.Terrestrials
{
    public class UpdateTerrestrial
    {
        public class UpdateTerrestrialCommand:IRequest<Terrestrial>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Color { set; get; }
            public int LegsNumber { get; set; }
            public bool IsFur { set; get; }
            public int TerrestrialTypeId { get; set; }
            public int AnimalTypeId { get; set; }
        }

        public class UpdateTerrestrialValidator : AbstractValidator<UpdateTerrestrialCommand>
        {
            public UpdateTerrestrialValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Color);
                RuleFor(x => x.LegsNumber);
                RuleFor(x => x.AnimalTypeId);
                RuleFor(x => x.TerrestrialTypeId);

            }
        }

        public class UpdateTerrestrialHandler:IRequestHandler<UpdateTerrestrialCommand,Terrestrial>

        {
            private readonly DataContext _context;

            public UpdateTerrestrialHandler(DataContext context)
            {
                _context = context;
            }

            public async Task<Terrestrial> Handle(UpdateTerrestrialCommand request, CancellationToken cancellationToken)
            {
                var terrestrial = await _context.Terrestrials.Include(x => x.AnimalType)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                if (terrestrial is null)
                {
                    throw new Exception("Terrestrial not found");
                }

                terrestrial.Name = request.Name;
                terrestrial.Color = request.Color;
                terrestrial.LegsNumber = request.LegsNumber;
                terrestrial.IsFur = request.IsFur;

                _context.Terrestrials.Update(terrestrial);
                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                {
                    throw new Exception("Error to update the terrestrial");
                }
                return terrestrial;
            }
        }
    }
}