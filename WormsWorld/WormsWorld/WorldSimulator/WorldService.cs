using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Hosting;

using WormsWorld.Enum;
using WormsWorld.Mover;
using WormsWorld.Entity;
using WormsWorld.Writer;
using WormsWorld.Generator.Food;
using WormsWorld.Generator.Name;

namespace WormsWorld.WorldSimulator
{
    public class WorldService : IHostedService
    {
        private readonly IWormMover _wormMover;
        private readonly IFileWriter _fileWriter;
        private readonly IFoodGenerator _foodGenerator;
        private readonly INameGenerator _nameGenerator;

        public readonly List<Worm> Worms = new();
        public readonly List<Food> Foods = new();

        public WorldService(IWormMover wormMover, IFileWriter fileWriter, IFoodGenerator foodGenerator, INameGenerator nameGenerator)
        {
            _wormMover = wormMover;
            _fileWriter = fileWriter;
            _foodGenerator = foodGenerator;
            _nameGenerator = nameGenerator;
            CreateWorm();
        }

        public Worm CreateWorm(int x = 0, int y = 0)
        {
            var name = _nameGenerator.GenerateName();
            while (IsWormNameExist(name))
            {
                name = _nameGenerator.GenerateName();
            }
            var worm = new Worm(this, _wormMover, name, x, y);
            Worms.Add(worm);
            return worm;
        }

        private bool IsWormNameExist(string name)
        {
            return Worms.Any(worm => worm.Name.Equals(name));
        }

        private void Start()
        {
            for (var day = 0; day < 100; day++)
            {
                _foodGenerator.GenerateFood(this);
                IsFoodEaten();
                WormAct();
                IsWormDead();
                CutFoodLifetime();
                IsFoodRotten();
                _fileWriter.WriteHistory(this);
            }
        }

        public void IsFoodEaten()
        {
            foreach (var worm in Worms)
            {
                var anyFood = Foods.FirstOrDefault(x => x.Position.Equals(worm.Position));
                if (anyFood != null)
                {
                    worm.Vitality += 10;
                    Foods.Remove(anyFood);
                }
            }
        }

        private void WormAct()
        {
            foreach (var worm in Worms.ToList())
            {
                var action = worm.GetWormAction();
                switch (action)
                {
                    case WormAction.Move:
                        worm.Move();
                        break;
                    case WormAction.Stay:
                        break;
                    case WormAction.Multiply:
                        Multiply(worm);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                worm.Vitality--;
            }
        }

        public void Multiply(Worm worm)
        {
            if (worm.Vitality > 10)
            {
                worm.Vitality -= 10;
                var direction = worm.GetMultiplyDirection();
                switch (direction)
                {
                    case Direction.Up:
                        if (IsCellFree(new Cell(worm.Position.X, worm.Position.Y + 1)))
                        {
                            CreateWorm(worm.Position.X, worm.Position.Y + 1);
                        }

                        break;
                    case Direction.Right:
                        if (IsCellFree(new Cell(worm.Position.X + 1, worm.Position.Y)))
                        {
                            CreateWorm(worm.Position.X + 1, worm.Position.Y);
                        }

                        break;
                    case Direction.Down:
                        if (IsCellFree(new Cell(worm.Position.X, worm.Position.Y - 1)))
                        {
                            CreateWorm(worm.Position.X, worm.Position.Y - 1);
                        }

                        break;
                    case Direction.Left:
                        if (IsCellFree(new Cell(worm.Position.X - 1, worm.Position.Y)))
                        {
                            CreateWorm(worm.Position.X - 1, worm.Position.Y);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool IsCellFree(Cell cell)
        {
            if (Worms.Any(worm => worm.Position.Equals(cell)))
            {
                return false;
            }
            if (Foods.Any(food => food.Position.Equals(cell)))
            {
                return false;
            }
            return true;
        }
        
        private void IsWormDead()
        {
            var deadWorms = Worms.ToList().Where(x => x.Vitality <= 0);
            foreach (var deadWorm in deadWorms)
            {
                Worms.Remove(deadWorm);
            }
        }

        private void CutFoodLifetime()
        {
            foreach (var food in Foods)
            {
                food.Lifetime--;
            }
        }

        private void IsFoodRotten()
        {
            var rottenFoods = Foods.ToList().Where(x => x.Lifetime <= 0);
            foreach (var rottenFood in rottenFoods)
            {
                Foods.Remove(rottenFood);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(Start, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}