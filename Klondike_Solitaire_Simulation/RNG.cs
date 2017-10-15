using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation
{
	public class RNG {
		private const int a = 2416;
		private const int c = 374441;
		private const int m = 1771875;

		/// <summary>
		/// The current seed.
		/// </summary>
		private int current_seed;

		public RNG(int seed = -1)
		{
			if (seed < 0)
			{
				TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
				current_seed = (int)span.TotalSeconds;
			}
			else
			{
				current_seed = seed;
			}
		}

		/// <summary>
		/// Get the next 'random' number in line.
		/// </summary>
		public int GetNumber()
		{
			// Recalculate the seed
			current_seed = (a * current_seed + c) % m;

			// Return the seed as number
			return current_seed;
		}

		/// <summary>
		/// Returns a 'random' number between 0 and 1.
		/// </summary>
		public float Next()
		{
			// Generate float number
			return GetNumber() / 1000000.0f;
		}

		/// <summary>
		/// Returns a 'random' number between 0 (inclusive) and max.
		/// </summary>
		/// <param name="max">The upper boundary (exclusive).</param>
		/// <returns>A random number.</returns>
		public int Next(int max)
		{
			return Next(0, max);
		}

		/// <summary>
		/// Returns a 'random' number between min and max.
		/// </summary>
		/// <param name="min">The lower boundary (inclusive).</param>
		/// <param name="max">The upper boundary (exclusive).</param>
		/// <returns>A random number.</returns>
		public int Next(int min, int max)
		{
			return min + GetNumber() % max;
		}
	}
}
