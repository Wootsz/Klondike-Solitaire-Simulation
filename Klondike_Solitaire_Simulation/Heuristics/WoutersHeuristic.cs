using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class WoutersHeuristic : Heuristic
    {
        public override State GetMove(State currentState, List<State> moves)
        {
            Rank highestRank = Rank.Ace;
            List<State> bestMoves = new List<State>();
            List<Card> moveAbleCards = new List<Card>();

            for(int i = 0; i < moveAbleCards.Count; i++)
            {
                if(moveAbleCards[i].Rank > highestRank)
                {
                    highestRank = moveAbleCards[i].Rank;
                    bestMoves = new List<State>() { moves[i] };
                }
                else if (moveAbleCards[i].Rank == highestRank)
                {
                    bestMoves.Add(moves[i]);
                }
            }

            //int[] scores = GetScore(currentState);
            //List<State> bestMoves = new List<State>();
            //int bestScore = 52;

            //foreach (State move in moves)
            //{
            //    int[] move_scores = GetScore(move);

            //    if(move_scores[flipIndex] < bestScore)
            //    {
            //        bestScore = move_scores[flipIndex];
            //        bestMoves = new List<State>() { move };
            //    }
            //    else if(move_scores[flipIndex] == bestScore)
            //    {
            //        bestMoves.Add(move);
            //    }
            //}

            return bestMoves[Utility.Random.Next(bestMoves.Count)];
        }
    }
}
