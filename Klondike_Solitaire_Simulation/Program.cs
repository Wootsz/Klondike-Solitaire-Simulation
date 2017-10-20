using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Klondike_Solitaire_Simulation.Stacks;
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

            WindowsHeuristic H1 = new WindowsHeuristic(r);
            while (true)
            {
                State state =  new State();//startState;

                // Make moves until you've reached an end state
                List<State> moves;
                while ((moves = state.GetMoves()).Count > 0)
                {
                    //state = moves[r.Next(moves.Count)];
                    state = H1.GetMove(state, moves);
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