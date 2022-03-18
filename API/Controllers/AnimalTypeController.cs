using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class AnimalTypeController : BaseController
    {
        private readonly DataContext _context;

        public AnimalTypeController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<AnimalType>>> ListAnimalType()
        {
            return await _context.AnimalTypes.ToListAsync();
        }

        [HttpGet("{description}")]
        public async Task<ActionResult<AnimalType>> GetAnimalTypeByDescription(string description)
        {
            var animalType = await _context.AnimalTypes.Where(x => x.Description.ToUpper() == description.ToUpper())
                .FirstOrDefaultAsync();
            if (animalType == null)
            {
                throw new Exception("Animal Type not found");
            }

            return animalType;
        }

        [HttpPost]
        public async Task<ActionResult<AnimalType>> AddAnimalType(AnimalType animalType)
        {
            var aType = new AnimalType
            {
                Description = animalType.Description
            };

            await _context.AnimalTypes.AddAsync(aType);
            var result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                throw new Exception("Fail to add new AnimalType");
            }

            return aType;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AnimalType>> UpdateAnimalType(int id, AnimalType animalType)
        {
            var aType = await _context.AnimalTypes.FindAsync(id);
            if (aType == null)
            {
                throw new Exception("Animal Type not found");
            }

            aType.Description = animalType.Description;
            _context.AnimalTypes.Update(aType);
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
            {
                throw new Exception("Fail to update the animal type");
            }
            
            return aType;
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnimalType>> RemoveAnimalType(int id)
        {
            var animalType = await _context.AnimalTypes.FindAsync(id);
            if (animalType == null)
            {
                throw new Exception("Animal Type not found");
            }

            _context.AnimalTypes.Remove(animalType);
            var result = await _context.SaveChangesAsync();
            if (result <= 0 )
            {
                throw new Exception("Fail to delete the animal type");
            }

            return animalType;
        }
    }
}