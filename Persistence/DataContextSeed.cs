using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class DataContextSeed
    {
        public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.AnimalTypes.Any())
                {
                    var animalTypeData = File.ReadAllText("../Persistence/SeedData/animalTypeSeed.json");
                    var animalTypes = JsonSerializer.Deserialize<List<AnimalType>>(animalTypeData);

                    foreach (var animalType in animalTypes)
                    {
                        await context.AddAsync(animalType);
                    }

                    await context.SaveChangesAsync();
                }
                
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<DataContext>();
                logger.LogError(e, e.Message);
            }
        }
    }
}