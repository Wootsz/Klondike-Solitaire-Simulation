﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class HighestRankHeuristic : Heuristic
    {
        public override State GetMove(State currentState, List<State> moves)
        {
            Rank highestRank = Rank.Ace;
            List<State> bestMoves = new List<State>();

            foreach (State move in moves)
            {
                if (move.moveAbleCard.Rank > highestRank)
                {
                    highestRank = move.moveAbleCard.Rank;
                    bestMoves = new List<State>() { move };
                }
                else if (move.moveAbleCard.Rank == highestRank)
                {
                    bestMoves.Add(move);
                }
            }

            return bestMoves[Utility.Random.Next(bestMoves.Count)];
        }
    }
}