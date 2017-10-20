using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Klondike_Solitaire_Simulation.Heuristics;

namespace Klondike_Solitaire_Simulation
{
	public class Program
	{
		private static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Console.WriteLine("Welcome to the Klondike Solitaire Simulator!");
			Console.Title = "Klondike Solitaire Simulator";

			Rng r = new Rng();

			// Initialize the game state
			//State startState = new State();

			//Console.WriteLine("Start state:");
			//Console.WriteLine(startState.ToString(false));

			//Console.WriteLine("Possible end states:");

			WindowsHeuristic H1 = new WindowsHeuristic();
            int H1Wincounter = 0;
            int iterations = 100;

			Parallel.For(0, iterations, index =>
			{
				State state = new State(); //startState;
				Console.WriteLine(index);

				// Make moves until you've reached an end state
				List<State> moves;
				while ((moves = state.GetMoves()).Count > 0)
				{
					if (!state.IsWinState)
						state = H1.GetMove(state, moves);
					else if (state.IsWinState)
					{
						H1Wincounter++;
						Console.WriteLine("Win");
						break;
					}

					//state = moves[r.Next(moves.Count)];

				}
				/*
				string stateText = state.ToString(true, 10);
				Console.WriteLine(stateText);
				StreamWriter writer = new StreamWriter(@"C:\Users\qub1\Desktop\Output.txt");
				writer.Write(stateText);
				writer.Close();
                */
			});

            Console.WriteLine((float)H1Wincounter/iterations * 100.0f + "%");
            Console.ReadLine();
        }
	}
}