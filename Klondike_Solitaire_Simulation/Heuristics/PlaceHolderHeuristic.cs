﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class PlaceHoldersHeuristic : Heuristic
    {
        public override State GetMove(State currentState, List<State> moves)
        {
            int[] currentScore = GetScore(currentState);
            int highScore = -52;
            List<State> newState = new List<State>();

            Rank lowestFoundationRank = Rank.King;
            for (int i = 1; i < 5; i++)
            {
                if (currentState.CardStacks[i].IsEmpty)
                {
                    lowestFoundationRank = Rank.Ace;
                    break;
                }
                else if (currentState.CardStacks[i].TopCard.Rank < lowestFoundationRank)
                    lowestFoundationRank = currentState.CardStacks[i].TopCard.Rank;
            }

            foreach (State move in moves)
            {
                int[] newScore = GetScore(move);
                int stockDif = newScore[stowasIndex] - currentScore[stowasIndex];
                int tableauxDif = newScore[tablIndex] - currentScore[tablIndex];
                int foundationDif = newScore[foundIndex] - currentScore[foundIndex];

                // Waste/stock to tableaux/foundations
                int score = 0;

                // Move to foundation
                if (foundationDif == 1)
                {
                    if (move.moveAbleCard.Rank >= (Rank)2)
                    {
                        if (lowestFoundationRank > (Rank)(move.moveAbleCard.Rank - 2))
                        {
                            score = -10;
                        }
                        else score = 10;
                    }
                    else score = 10;
                }

                // Stock to tableaux
                if (tableauxDif == 1 && stockDif == -1)
                {
                    score = 1;
                }

                // Turn over a tableau card
                if (newScore[flipIndex] < currentScore[flipIndex] || tableauxDif == -1 && newScore[flipIndex] == currentScore[flipIndex])
                {
                    score += 1;
                }

                if (score > highScore)
                {
                    score = highScore;
                    newState = new List<State> { move };
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
