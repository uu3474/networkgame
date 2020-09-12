using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine
{
    public static class StaticRandom
    {
        static Random rnd;

        static StaticRandom()
        {
            rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        public static int Next(int maxValue)
        {
            return rnd.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }
    }
}
