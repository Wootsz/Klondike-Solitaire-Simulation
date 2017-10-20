using Klondike_Solitaire_Simulation.Stacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class BaseHeuristic
    {
        public RNG r;

        /// <summary>
        /// Creates a new BaseHeuristic
        /// </summary>
        /// <param name="r"></param>
        public BaseHeuristic(RNG r)
        {
            this.r = r;
        }

        public virtual State GetMove(State currentState, List<State> moves) { return null; }


        /// <summary>
        /// Count the amount of cards in the stock/waste, foundations and tableaux
        /// </summary>
        /// <param name="s">The state you want to count for</param>
        /// <returns>An integer array with: 0 = waste/stock-count, 1 = tableaux-count, 2 = foundations-count, 3 = amount of flipped cards</returns>
        public static int[] GetScore(State s)
        {
            int foundationScore = 0, tableauScore = 0, faceDownCount = 0;
            foreach (CardStack tableau in s.Tableaus)
            {
                tableauScore += tableau.CardCount;
                faceDownCount += tableau.FlippedCardCount;
            }
            foreach (CardStack foundation in s.Foundations)
            {
                foundationScore += foundation.CardCount;
            }
            return new int[4] { s.Stock.CardCount + s.Stock.Waste.CardCount, tableauScore, foundationScore, faceDownCount };
        }
    }
}
