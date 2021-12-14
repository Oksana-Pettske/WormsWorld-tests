using WormsWorld.WorldSimulator;

namespace WormsWorld.Generator.Food
{
    public interface IFoodGenerator
    {
        void GenerateFood(WorldService worldService);
    }
}