using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class PlaceHolderHeuristic2 : Heuristic
    {
        public override State GetMove(State currentState, List<State> moves)
        {
            int[] scores1 = GetScore(currentState);
            int highScore = -52;
            List<State> bestStates = new List<State>();

            foreach (State move in moves)
            {
                int[] scores = GetScore(move);
                int stockDif = scores[stowasIndex] - scores1[stowasIndex];
                int tableauxDif = scores[tablIndex] - scores1[tablIndex];
                int foundationDif = scores[foundIndex] - scores1[foundIndex];

                int score = 0;

                if(stockDif == -1 && tableauxDif == 1 && ExistsFollowUpCard(move))
                {
                    score = 30;
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

        public bool ExistsFollowUpCard(State nextState)
        {
            for (int i = 1; i < 5; i++)
            {
                Card a = nextState.CardStacks[i].TopCard; // .BottomStackCard;
                if(a.Rank == nextState.moveAbleCard.Rank - 1 && nextState.moveAbleCard.OppositeColor(a.Suit))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
