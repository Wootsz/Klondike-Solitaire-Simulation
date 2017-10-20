using System;

namespace Klondike_Solitaire_Simulation
{
	public class RandomNumberGenerator
	{
		private const int A = 2416;
		private const int C = 374441;
		private const int M = 1771875;

		/// <summary>
		/// The current seed.
		/// </summary>
		private long _currentSeed;

		/// <summary>
		/// Creates a new random generator.
		/// </summary>
		/// <param name="seed">The given seed.</param>
		public RandomNumberGenerator(long seed = -1)
		{
			// Check if a seed has been given
			if (seed < 0)
			{
				TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
				_currentSeed = (long) span.TotalMilliseconds;
			}
			else
			{
				_currentSeed = seed;
			}
		}

		/// <summary>
		/// Get the next 'random' number in line.
		/// </summary>
		public long GetNumber() => _currentSeed = (A * _currentSeed + C) % M;

		/// <summary>
		/// Returns a 'random' number between 0 and 1.
		/// </summary>
		public float Next() => GetNumber() / 1000000.0f;

		/// <summary>
		/// Returns a 'random' number between 0 (inclusive) and max.
		/// </summary>
		/// <param name="max">The upper boundary (exclusive).</param>
		/// <returns>A random number.</returns>
		public long Next(int max) => Next(0, max);

		/// <summary>
		/// Returns a 'random' number between min and max.
		/// </summary>
		/// <param name="min">The lower boundary (inclusive).</param>
		/// <param name="max">The upper boundary (exclusive).</param>
		/// <returns>A random number.</returns>
		public long Next(int min, int max) => min + GetNumber() % max;
	}
}