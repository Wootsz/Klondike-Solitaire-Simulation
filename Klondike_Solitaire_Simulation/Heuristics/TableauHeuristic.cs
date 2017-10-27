﻿using System.Collections.Generic;

namespace Klondike_Solitaire_Simulation.Heuristics
{
	class TableauHeuristic : Heuristic
	{
		public override State GetMove(State currentState, List<State> moves)
		{
			int[] currentScore = GetScore(currentState);
			int highScore = 0;
			List<State> newState = new List<State>();

			foreach (State move in moves)
			{
				int[] newScore = GetScore(move);
				int stockDif = newScore[stowasIndex] - currentScore[stowasIndex];
				int tableauxDif = newScore[tablIndex] - currentScore[tablIndex];
				int foundationDif = newScore[foundIndex] - currentScore[foundIndex];

				// Waste/stock to tableaux/foundations
				int score = 0;

				//Tableaux move
				if (tableauxDif == 0 && foundationDif == 0 && stockDif == 0)
				{
					score = 1;
				}

				// Tableaux to foundations
				if (tableauxDif == -1 && foundationDif == 1)
				{
					score = 15;
				}

				// Stock to tableaux
				if (tableauxDif == 1 && stockDif == -1)
				{
					score = 10;
				}

				// Turn over a tableau card
				if (newScore[flipIndex] < currentScore[flipIndex] || tableauxDif == -1 && newScore[flipIndex] == currentScore[flipIndex])
				{
					score += 20;
				}

				if (score > highScore)
				{
                    score = highScore;
					newState = new List<State> {move};
				}
				else if (score == highScore)
				{
					newState.Add(move);
				}
			}

			return newState[Utility.Random.Next(newState.Count)];
		}
	}
}