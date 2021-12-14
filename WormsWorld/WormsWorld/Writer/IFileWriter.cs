using System;
using WormsWorld.WorldSimulator;

namespace WormsWorld.Writer
{
    public interface IFileWriter : IDisposable
    {
        void WriteHistory(WorldService worldService);
    }
}