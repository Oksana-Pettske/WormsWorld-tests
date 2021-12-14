using System.IO;
using System.Linq;
using WormsWorld.WorldSimulator;

namespace WormsWorld.Writer
{
    public class FileWriter : IFileWriter
    {
        private readonly StreamWriter _streamWriter;

        public FileWriter(string path)
        {
            _streamWriter = new StreamWriter(path);
        }
        public void WriteHistory(WorldService worldService)
        {
            _streamWriter.Write("Worms: [");
            foreach (var worm in worldService.Worms)
            {
                _streamWriter.Write(worm.Name + " (" + worm.Position.X + ", " + worm.Position.Y + ")], ");
            }
            _streamWriter.Write("Food: [");
            var i = 0;
            foreach (var food in worldService.Foods.ToList())
            {
                _streamWriter.Write("(" + food.Position.X + ", " + food.Position.Y + ")");
                i++;
                if (i < worldService.Foods.Count)
                {
                    _streamWriter.Write(", ");
                }
            }
            _streamWriter.Write("]\n");
        }

        public void Dispose()
        {
            _streamWriter?.Dispose();
        }
    }
}