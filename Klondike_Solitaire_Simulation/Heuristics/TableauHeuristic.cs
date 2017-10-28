using Klondike_Solitaire_Simulation.Stacks;
using System.Collections.Generic;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class TableauHeuristic : Heuristic
    {
        int highScore = 0;
        List<State> newState = new List<State>();

        public override State GetMove(State currentState, List<State> moves)
        {
            int[] currentScore = GetScore(currentState);
            int turnTableau = 90;
            int stockToTableau = 80;
            int tableauMove = 70;
            int tableauToFoundations = 60;

            foreach (State move in moves)
            {
                //Foundation score
                int foundationScore = 0;
                foreach (CardStack foundation in move.Foundations)
                {
                    foundationScore += foundation.CardCount;
                }

                if (foundationScore > 10)
                {
                    tableauMove = 60;
                    tableauToFoundations = 70;
                }

                int[] newScore = GetScore(move);
                int stockDif = newScore[stowasIndex] - currentScore[stowasIndex];
                int tableauxDif = newScore[tablIndex] - currentScore[tablIndex];
                int foundationDif = newScore[foundIndex] - currentScore[foundIndex];

                //If you can move an Ace or Two always do it
                if (highScore <= 100 && (move.moveAbleCard.Rank == Rank.Ace || move.moveAbleCard.Rank == Rank.Two))
                {
                    handleNextStateScore(move, 100);
                    highScore = 100;
                }

                // Turn over a tableau card
                if (highScore <= turnTableau && (newScore[flipIndex] < currentScore[flipIndex] || tableauxDif == -1 && newScore[flipIndex] == currentScore[flipIndex]))
                {
                    handleNextStateScore(move, turnTableau);
                    highScore = turnTableau;
                }

                // Stock to tableaux
                if (highScore <= stockToTableau && (tableauxDif == 1 && stockDif == -1))
                {
                    handleNextStateScore(move, stockToTableau);
                    highScore = stockToTableau;
                }

                //Tableaux move
                if (highScore <= tableauMove && (tableauxDif == 0 && foundationDif == 0 && stockDif == 0))
                {
                    handleNextStateScore(move, tableauMove);
                    highScore = tableauMove;
                }

                // Tableaux to foundations
                if (highScore <= tableauToFoundations && (tableauxDif == -1 && foundationDif == 1))
                {
                    handleNextStateScore(move, tableauToFoundations);
                    highScore = tableauToFoundations;
                }


            }
            return newState[Utility.Random.Next(newState.Count)];
        }

        public void handleNextStateScore(State state, int score)
        {
            if (highScore < score)
            {
                newState = new List<State>() { state };
            }
        }
    }
}