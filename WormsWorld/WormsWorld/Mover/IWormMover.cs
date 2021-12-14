using WormsWorld.Entity;

namespace WormsWorld.Mover
{
    public interface IWormMover
    {
        Cell Move(Worm worm);
    }
}