using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WormsWorld.Mover;
using WormsWorld.Writer;
using WormsWorld.Generator.Food;
using WormsWorld.Generator.Name;
using WormsWorld.WorldSimulator;

namespace WormsWorld
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<WorldService>();
                    services.AddSingleton<IFoodGenerator, FoodGenerator>();
                    services.AddSingleton<IFileWriter>(_ => new FileWriter("AboutWorms.txt"));
                    services.AddSingleton<INameGenerator, NameGenerator>();
                    services.AddSingleton<IWormMover, WormMover>();
                });
        }
    }
}