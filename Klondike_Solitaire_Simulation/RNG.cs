using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onderzoeksmethoden
{
    class RNG
    {
        private int a, c, m;
        private int i;

        public RNG(int seed = 0, bool is_seed = false)
        {
            a = 2416;
            c = 374441;
            m = 1771875;

            if (!is_seed)
            {
                TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
                i = (int)span.TotalSeconds;
            }
            else
                i = seed;
        }

        /// <summary>
        /// Get the next 'random' number in line
        /// </summary>
        public int GetNumber()
        {
            i = (a * i + c) % m;
            return i;
        }

        /// <summary>
        /// Returns a 'random' number between 0 and 1
        /// </summary>
        public float Next() { return GetNumber() / 1000000; }

        /// <summary>
        /// Returns a 'random' number between 0 and max
        /// </summary>
        /// <param name="max">Exclusive</param>
        /// <returns> 0 <= number < max</returns>
        public int Next(int max) { return GetNumber() % max; }
    }
}
