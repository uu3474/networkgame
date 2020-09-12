using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Logic
{
    public class LevelDescriptor
    {
        public const int DifficultsCount = 4;

        public int Index { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Difficult { get; protected set; }

        public LevelDescriptor(int index, int width, int height, int difficult)
        {
            this.Index = index;
            this.Width = width;
            this.Height = height;
            this.Difficult = difficult;
        }
    }
}
