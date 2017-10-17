using System;
using System.Collections.Generic;
using System.IO;
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

				// Make a few moves
				Random r = new Random();
				List<State> moves;
				while ((moves = state.GetMoves()).Count > 0)
				{
					state = moves[r.Next(moves.Count)];
				}

				string stateText = state.ToString(true, 10);
				Console.WriteLine(stateText);
				StreamWriter writer = new StreamWriter(@"C:\Users\qub1\Desktop\Output.txt");
				writer.Write(stateText);
			}

			Console.ReadLine();
		}
	}
}