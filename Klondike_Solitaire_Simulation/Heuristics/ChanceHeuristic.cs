using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class ChanceHeuristic : Heuristic
    {
        public int stockToTab = 5, stockToFou = 10, tabToFou = 20, tabToTab = 0, fouToTab = 0;

        public override State GetMove(State currentState, List<State> moves)
        {
            int[] currentScore = GetScore(currentState);
            Dictionary<int, List<State>> states = new Dictionary<int, List<State>>();

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

                // Always move an the lowest card to the foundation immediately
                if (foundationDif == 1 && move.moveAbleCard.Rank == lowestFoundationRank)
                    return move;

                int score = 0;

                if (tableauxDif == -1 && foundationDif == 1)
                    score = tabToFou;
                if (stockDif == -1 && foundationDif == 1)
                    score = stockToFou;
                if (foundationDif == -1 && tableauxDif == 1)
                    score = fouToTab;
                if (stockDif == -1 && tableauxDif == 1)
                    score = stockToTab;
                if (tableauxDif == 0 && stockDif == 0 && foundationDif == 0)
                    score = tabToTab;

                if (states.ContainsKey(score))
                    states[score].Add(move);
                else
                    states.Add(score, new List<State>() { move });

            }

            return ChooseState(states);
        }

        public State ChooseState(Dictionary<int, List<State>> states)
        {
            int p = Utility.Random.Next(100);

            // 70%: Select a random best move
            if (p < 90)
                return states[states.Keys.Max()][Utility.Random.Next(states[states.Keys.Max()].Count)];
            // 25%: Select a random second best move
            if (p < 95)
            {
               Dictionary<int, List<State>> sndBestStates = new Dictionary<int, List<State>>(states);
                sndBestStates.Remove(states.Keys.Max());
                if (sndBestStates.Count != 0)
                    return sndBestStates[sndBestStates.Keys.Max()][Utility.Random.Next(sndBestStates[sndBestStates.Keys.Max()].Count)];
                
            }
            // 10%: Select random move
            int random = Utility.Random.Next(states.Keys.Count);
            int key = states.Keys.ToList()[random];
            return states[key][Utility.Random.Next(states[key].Count)];

        }
    }
}
