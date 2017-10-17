using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Console.WriteLine("Welcome to the Klondike Solitaire Simulator!");
			Console.Title = "Klondike Solitaire Simulator";

			int iterations = 1;

			for (int iteration = 0; iteration < iterations; iteration++)
			{
				// Initialize the game state
				State state = new State();

				Console.WriteLine(state.ToString(true));
			}

			Console.ReadLine();
		}
	}
}