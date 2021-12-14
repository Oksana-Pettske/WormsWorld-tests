using System;
using WormsWorld.Entity;

namespace WormsWorld.Mover
{
    public class WormMover : IWormMover
    {
        public Cell Move(Worm worm)
        {
            var closestFoodCell = FindClosestFoodCell(worm);
            if (closestFoodCell.X > worm.Position.X)
            {
                worm.Position.X++;
            }
            else if (closestFoodCell.X < worm.Position.X)
            {
                worm.Position.X--;
            }
            else if (closestFoodCell.Y > worm.Position.Y)
            {
                worm.Position.Y++;
            }
            else if (closestFoodCell.Y < worm.Position.Y)
            {
                worm.Position.Y--;
            }
            return worm.Position;
        }

        private Cell FindClosestFoodCell(Worm worm)
        {
            var minDistance = int.MaxValue;
            var closestFoodCell = new Cell();
            foreach (var food in worm.WorldService.Foods)
            {
                var deltaX = Math.Abs(food.Position.X - worm.Position.X);
                var deltaY = Math.Abs(food.Position.Y - worm.Position.Y);
                if (deltaX + deltaY < minDistance)
                {
                    closestFoodCell.X = food.Position.X;
                    closestFoodCell.Y = food.Position.Y;
                    minDistance = deltaX + deltaY;
                }
            }
            return closestFoodCell;
        }
    }
}