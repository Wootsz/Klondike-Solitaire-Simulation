using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Klondike_Solitaire_Simulation.Stacks;

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
			while (true)
			{
				State state = new State(); //startState;

				// Make moves until you've reached an end state
				List<State> moves;
				while ((moves = state.GetMoves()).Count > 0)
				{
					//state = moves[r.Next(moves.Count)];
					state = WindowsScoreHeuristic(state, moves, r);
				}

				string stateText = state.ToString(true, 10);
				Console.WriteLine(stateText);
				StreamWriter writer = new StreamWriter(@"C:\Users\qub1\Desktop\Output.txt");
				writer.Write(stateText);
				writer.Close();

				Console.ReadLine();
			}
		}

		static State WindowsScoreHeuristic(State state1, List<State> moves, Rng r)
		{
			int[] scores1 = GetScore(state1);
			int highScore = -20;
			List<State> bestStates = new List<State>();
			foreach (State move in moves)
			{
				int[] scores = GetScore(move);
				int tableauxDif = scores[1] - scores1[1];
				int foundationDif = scores[2] - scores1[2];

				// Waste/stock to tableaux/foundations
				int score = Math.Abs(scores[0] - scores1[0]) * (5 * tableauxDif + 10 * foundationDif);
				// Tableaux to foundations
				if (tableauxDif == -1 && foundationDif == 1)
					score = 10;
				// Foundations to tableaux
				if (tableauxDif == 1 && foundationDif == -1)
					score = -15;
				// Turn over a tableau card
				if (scores[3] < scores1[3] || (tableauxDif == -1 && scores[3] == scores1[3]))
					score += 5;

				if (score > highScore)
					bestStates = new List<State>() { move };
				else if (score == highScore)
					bestStates.Add(move);
			}

			return bestStates[(int) r.Next(bestStates.Count)];
		}

		/// <summary>
		/// Count the amount of cards in the stock/waste, foundations and tableaux
		/// </summary>
		/// <param name="s">The state you want to count for</param>
		/// <returns>An integer array with: 0 = waste/stock-count, 1 = tableaux-count, 2 = foundations-count, 3 = amount of flipped cards</returns>
		static int[] GetScore(State s)
		{
			int foundationScore = 0, tableauScore = 0, faceDownCount = 0;

			foreach (TableauCardStack tableau in s.Tableaus)
			{
				tableauScore += tableau.CardCount;
				faceDownCount += tableau.FlippedCardCount;
			}

			foundationScore += s.Foundations.Sum(foundation => foundation.CardCount);

			return new[] { s.Stock.CardCount + s.Stock.Waste.CardCount, tableauScore, foundationScore, faceDownCount };
		}
	}
}