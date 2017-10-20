using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class WindowsHeuristic : BaseHeuristic
    {
        /// <summary>
        /// Creates a new Windows Heuristic 
        /// </summary>
        /// <param name="r">RNG</param>
        public WindowsHeuristic(RNG r) : base(r)
        {
        }

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
            return bestStates[(int)r.Next(bestStates.Count)];
        }
    }
}
