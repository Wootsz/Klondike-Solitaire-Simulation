using Klondike_Solitaire_Simulation.Stacks;
using System.Collections.Generic;
using System.Linq;

namespace Klondike_Solitaire_Simulation.Heuristics
{
	class BaseHeuristic
	{
		public virtual State GetMove(State currentState, List<State> moves) => null;

		/// <summary>
		/// Count the amount of cards in the stock/waste, foundations and tableaux
		/// </summary>
		/// <param name="s">The state you want to count for</param>
		/// <returns>An integer array with: 0 = waste/stock-count, 1 = tableaux-count, 2 = foundations-count, 3 = amount of flipped cards</returns>
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