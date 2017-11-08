using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class WouterHeuristic : Heuristic
    {
        public override State GetMove(State currentState, List<State> moves)
        {
            int[] scores1 = GetScore(currentState);
            int highScore = -52;
            List<State> bestStates = new List<State>();

            Rank lowestFoundationRank = Rank.King;
            for (int i = 2; i < 5; i++)
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
                int[] scores = GetScore(move);
                int stockDif = scores[stowasIndex] - scores1[stowasIndex];
                int tableauxDif = scores[tablIndex] - scores1[tablIndex];
                int foundationDif = scores[foundIndex] - scores1[foundIndex];

                // Always move an the lowest card to the foundation immediately
                if (foundationDif == 1 && move.moveAbleCard.Rank == lowestFoundationRank)
                    return move;

                int score = 0;

                if (stockDif == -1 && tableauxDif == 1)
                    if (ExistsFollowUpCard(move))
                        score = 10;
                    else score = -1;

                if (tableauxDif == -1 && foundationDif == 1)
                    score = 20;

                if (stockDif == -1 && foundationDif == 1)
                    if (move.moveAbleCard.Rank <= lowestFoundationRank + 2)
                        score = 20;
                    else score = -1;

                if (foundationDif == -1 && tableauxDif == 1)
                    score = -2;

                if (tableauxDif == 0 && stockDif == 0 && foundationDif == 0)
                    score = -1;

                if (scores[flipIndex] < scores1[flipIndex] || tableauxDif == -1 && scores[flipIndex] == scores1[flipIndex])
                    score += 5;

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
            for (int i = 6; i < 12; i++)
            {
                if (!nextState.CardStacks[i].IsEmpty)
                {
                    Card a = nextState.CardStacks[i].UnflippedCards.Last();
                    if (a.Rank == nextState.moveAbleCard.Rank - 1 && nextState.moveAbleCard.OppositeColor(a.Suit))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
