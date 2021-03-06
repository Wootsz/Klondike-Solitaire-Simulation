﻿using Klondike_Solitaire_Simulation.Stacks;
using System.Collections.Generic;
using System.Linq;

namespace Klondike_Solitaire_Simulation.Heuristics
{
	abstract class Heuristic
	{
        public int stowasIndex = 0, tablIndex = 1, foundIndex = 2, flipIndex = 3;

		public abstract State GetMove(State currentState, List<State> moves);

        /// <summary>
        /// Count the amount of cards in the stock/waste, foundations and tableaux, 
        /// returns an integer array with: 0 = waste/stock-count, 1 = tableaux-count, 2 = foundations-count, 3 = amount of flipped cards
        /// </summary>
        /// <param name="s">The state you want to count for</param>
        public static int[] GetScore(State s)
		{
			int foundationScore = 0, tableauScore = 0, faceDownCount = 0;
			foreach (TableauCardStack tableau in s.Tableaus)
			{
				tableauScore += tableau.CardCount;
				faceDownCount += tableau.FlippedCardCount;
			}

			foundationScore += s.Foundations.Sum(foundation => foundation.CardCount);

			return new[]
			{
				s.Stock.CardCount + s.Stock.Waste.CardCount,
				tableauScore,
				foundationScore,
				faceDownCount
			};
		}
	}
}