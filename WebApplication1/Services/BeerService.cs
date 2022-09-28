using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class BeerService : IBeerService
    {
        string path = "C:\\Users\\FrancoCamposaragna\\source\\repos\\WebApplication1\\WebApplication1\\beers.json";

        public async Task<List<Beer>> Get()
        {
            string content = await File.ReadAllTextAsync(path);
            var beers = JsonSerializer.Deserialize<List<Beer>>(content);
            return beers;
        }
    }
}
