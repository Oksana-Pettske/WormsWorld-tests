using System;
using WormsWorld.Util;
using WormsWorld.Entity;
using WormsWorld.WorldSimulator;

namespace WormsWorld.Generator.Food
{
    public class FoodGenerator : IFoodGenerator
    {
        public void GenerateFood(WorldService worldService)
        {
            Cell cell;
            var done = true;
            while (true)
            {
                cell = GenerateCell();
                foreach (var food in worldService.Foods)
                {
                    if (cell.Equals(food.Position))
                    {
                        done = false;
                        break;
                    }
                    done = true;
                }
                if (done)
                {
                    break;
                }
            }
            worldService.Foods.Add(new Entity.Food(cell));
        }
        
        private static Cell GenerateCell()
        {
            var random = new Random();
            var x = NormalDistribution.NextNormal(random);
            var y = NormalDistribution.NextNormal(random);
            return new Cell(x, y);
        }
    }
}