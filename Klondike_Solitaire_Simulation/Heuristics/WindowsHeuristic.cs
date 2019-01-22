using System;
using System.Collections.Generic;

namespace Klondike_Solitaire_Simulation.Heuristics
{
	class WindowsHeuristic : Heuristic
	{
		/// <summary>
		/// Get your next move based on this Heuristic
		/// </summary>
		/// <param name="currentState"></param>
		/// <param name="moves"></param>
		/// <returns></returns>
		public override State GetMove(State currentState, List<State> moves)
		{
			int[] scores1 = GetScore(currentState);
			int highScore = -20;
			List<State> bestStates = new List<State>();

			foreach (State move in moves)
			{
				int[] scores = GetScore(move);
				int tableauxDif = scores[tablIndex] - scores1[tablIndex];
				int foundationDif = scores[foundIndex] - scores1[foundIndex];

				int score;
				if (tableauxDif == -1 && foundationDif == 1)
				{
					// Tableaux to foundations
					score = 10;
				}
				else if (tableauxDif == 1 && foundationDif == -1)
				{
					// Foundations to tableaux
					score = -15;
				}
				else
				{
					// Waste/stock to tableaux/foundations
					score = Math.Abs(scores[stowasIndex] - scores1[stowasIndex]) * (5 * tableauxDif + 10 * foundationDif);
				}

				// Turn over a tableau card
				if (scores[flipIndex] < scores1[flipIndex] || tableauxDif == -1 && scores[flipIndex] == scores1[flipIndex])
				{
					score += 5;
				}

				if (score > highScore)
				{
					bestStates = new List<State> { move };
					highScore = score;
				}
				else if (score == highScore)
				{
					bestStates.Add(move);
				}
			}

			return bestStates[Utility.Random.Next(bestStates.Count)];
		}
	}
}