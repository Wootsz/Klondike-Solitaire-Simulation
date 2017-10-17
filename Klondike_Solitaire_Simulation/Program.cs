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

			Random r = new Random();

			// Initialize the game state
			State startState = new State();

			Console.WriteLine("Start state:");
			Console.WriteLine(startState.ToString(false));

			Console.WriteLine("Possible end states:");
			while (true)
			{
				State state = startState;

				// Make a few moves
				List<State> moves;
				while ((moves = state.GetMoves()).Count > 0)
				{
					state = moves[r.Next(moves.Count)];
				}

				string stateText = state.ToString(true, 10);
				Console.WriteLine(stateText);
				StreamWriter writer = new StreamWriter(@"C:\Users\qub1\Desktop\Output.txt");
				writer.Write(stateText);
				writer.Close();

				Console.ReadLine();
			}
		}
	}
}