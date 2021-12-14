using System;
using WormsWorld.Enum;
using WormsWorld.Mover;
using WormsWorld.WorldSimulator;

namespace WormsWorld.Entity
{
    public class Worm
    {
        public WorldService WorldService { get; }
        private readonly IWormMover _wormMover;
        
        public readonly string Name;
        public Cell Position;
        public int Vitality;

        public Worm(WorldService worldService, IWormMover wormMover, string name, int x, int y)
        {
            WorldService = worldService;
            _wormMover = wormMover;
            Name = name;
            Position.X = x;
            Position.Y = y;
            Vitality = 10;
        }

        public Cell Move()
        {
            return _wormMover.Move(this);
        }

        public WormAction GetWormAction()
        {
            var values = System.Enum.GetValues(typeof(WormAction));
            var random = new Random();
            return (WormAction) values.GetValue(random.Next(values.Length));
        }

        public Direction GetMultiplyDirection()
        {
            var values = System.Enum.GetValues(typeof(Direction));
            var random = new Random();
            return (Direction) values.GetValue(random.Next(values.Length));
        }
    }
}