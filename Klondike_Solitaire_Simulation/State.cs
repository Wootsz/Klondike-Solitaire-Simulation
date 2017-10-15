using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
    public class State
    {
        /// <summary>
        /// The cards left in the stock.
        /// </summary>
        public CardStack stock = CardStack.GenerateStandardDeck();

        /// <summary>
        /// The current waste (cards that can be moved to tableaus).
        /// </summary>
        public CardStack waste = new CardStack();

        /// <summary>
        /// The foundations where the final piles can be placed.
        /// </summary>
        public List<CardStack> foundations = Enumerable.Repeat(new CardStack(), 4).ToList();

        /// <summary>
        /// The actual game stacks where the cards are moved to and from.
        /// </summary>
        public List<CardStack> tableaus = Enumerable.Repeat(new CardStack(), 7).ToList();

        /// <summary>
        /// Creates card new game state.
        /// </summary>
        public State()
        {
            // Shuffle the stock (the entire deck)
            stock.Shuffle();

            // Fill tableaus
            for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
            {
                stock.MoveFromTop(tableaus[tableauIndex], tableauIndex + 1);
            }
        }

        /// <summary>
        /// Gets card string representation of the current state.
        /// </summary>
        /// <returns>The current state as card string.</returns>
        public override string ToString()
        {
            string result = "";

            // Output stacks
            result += "Stock: " + stock.ToString() + "\n";

            result += "Waste: " + waste.ToString() + "\n";

            foreach (CardStack foundation in foundations)
            {
                result += "Foundation: " + foundation.ToString() + "\n";
            }

            foreach (CardStack tableau in tableaus)
            {
                result += "Tableau: " + tableau.ToString() + "\n";
            }

            result += "\n";

            return result;
        }

        /// <summary>
        /// Get a list of all possible moves
        /// </summary>
        /// <returns>List of all next-states</returns>
        public List<State> GetMoves()
        {
            List<State> nextStates = new List<State>();

            // 1. move from stack to tableau
            // 2. move from stack to foundation
            // 3. move from tableau to tableau
            // 4. move from tableau to foundation
            // 5. move from foundation to tableau

            List<Card> availableStockCards = new List<Card>();
            while(!stock.IsEmpty())
            {
                for (int stockIndex = 0; stockIndex < Math.Max(3, stock.Count()); stockIndex++)
                {
                    waste.Push(stock.Pop());
                }
                availableStockCards.Add(waste.Peek());
            }
            foreach(Card card in availableStockCards)
            {
                foreach(int index in FindTableaus(card))
                {
                    State nextState = this;
                    nextState.stock.Remove(card);
                    nextState.tableaus[index].Push(card);
                    // if(!stateHistory.Contains(nextState))
                    nextStates.Add(nextState);
                }
                int foundationIndex = FindFoundation(card);
                if (foundationIndex != -1)
                {
                    State nextState = this;
                    nextState.stock.Remove(card);
                    nextState.foundations[foundationIndex].Push(card);
                    // if(!stateHistory.Contains(nextState))
                    nextStates.Add(nextState);
                }
            }

            for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
            {
                foreach (int index in FindTableaus(tableaus[tableauIndex].Peek()))
                {
                    if (index != tableauIndex)
                    {
                        State nextState = this;
                        nextState.tableaus[index].Push(nextState.tableaus[tableauIndex].Pop());
                        // if(!stateHistory.Contains(nextState))
                        nextStates.Add(nextState);
                    }
                }
                int foundationIndex = FindFoundation(tableaus[tableauIndex].Peek());
                if (foundationIndex != -1)
                {
                    State nextState = this;
                    nextState.foundations[foundationIndex].Push(nextState.tableaus[tableauIndex].Pop());
                    // if(!stateHistory.Contains(nextState))
                    nextStates.Add(nextState);
                }
            }

            for (int foundationIndex = 0; foundationIndex < foundations.Count; foundationIndex++)
            {
                foreach (int index in FindTableaus(foundations[foundationIndex].Peek()))
                {
                    State nextState = this;
                    nextState.tableaus[index].Push(nextState.foundations[foundationIndex].Pop());
                    // if(!stateHistory.Contains(nextState))
                    nextStates.Add(nextState);

                }
            }

            return nextStates;
        }

        /// <summary>
		/// Check on which tableaus you can put a card
		/// </summary>
		/// <param name="card">>Card you want to move</param>
        /// <returns>The indices of the tableaus you can place the card on</returns>
		public List<int> FindTableaus(Card card)
        {
            List<int> indices = new List<int>();
            for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
            {

                if (tableaus[tableauIndex].IsEmpty())
                {
                    if (card.rank == Rank.King)
                        return indices = new List<int> { tableauIndex };
                }
                else
                {
                    Card bottom = tableaus[tableauIndex].Peek().TopCard();

                    if (card.rank == bottom.rank - 1)
                    {
                        if (card.suit == Diamonds || bottom.suit == Hearts)
                        {
                            if (bottom.suit == Clubs || bottom.suit == Spades)
                                indices.Add(tableauIndex);
                        }
                        else
                        {
                            if (bottom.suit == Diamonds || bottom.suit == Hearts)
                                indices.Add(tableauIndex);
                        }


                    }
                }
            }

            return indices;
        }

        /// <summary>
        /// Check which foundation you can place a card on
        /// </summary>
        /// <param name="card">Card you want to move</param>
        /// <returns>The index of the foundation you can place the card on</returns>
        public int FindFoundation(Card card)
        {
            for (int foundationIndex = 0; foundationIndex < foundations.Count; foundationIndex++)
            {
                if (foundations[foundationIndex].IsEmpty() && card.rank == A || card.suit == foundations[foundationIndex].Peek().suit && card.rank == foundations[foundationIndex].Peek().rank - 1)
                {
                    return foundationIndex;
                }
            }

            return -1;
        }
    }
}
