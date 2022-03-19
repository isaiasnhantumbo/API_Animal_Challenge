using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Features.TerrestrialTypes
{
    public class AddTerrestrialTypes
    {
        public class AddTerrestrialTypesCommand : IRequest<TerrestrialType>
        {
            public string Description { get; set; }
        }
        
        public class AddTerrestrialTypesValidator : AbstractValidator<AddTerrestrialTypesCommand>
        {
            public AddTerrestrialTypesValidator()
            {
                RuleFor(x => x.Description).NotEmpty();
            }
        }
        
        public class AddTerrestrialTypesHandler : IRequestHandler<AddTerrestrialTypesCommand, TerrestrialType>
        {
            private readonly DataContext _context;

            public AddTerrestrialTypesHandler(DataContext context)
            {
                _context = context;
            }
            public async Task<TerrestrialType> Handle(AddTerrestrialTypesCommand request, CancellationToken cancellationToken)
            {
                var terrestrialType = await _context.TerrestrialTypes
                    .Where(x => x.Description.ToUpper() == request.Description.ToUpper())
                    .FirstOrDefaultAsync();
                if (terrestrialType is not null)
                {
                    throw new Exception("TerrestrialType exist in data base");
                }

                terrestrialType = new TerrestrialType
                {
                    Description = request.Description
                };

                await _context.TerrestrialTypes.AddAsync(terrestrialType);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                {
                    throw new Exception("Fail to add Terrestrial Type");
                }

                return terrestrialType;
            }
        }
    }
}