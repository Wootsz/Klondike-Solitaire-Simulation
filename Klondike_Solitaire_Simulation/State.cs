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

        const char tableauChar = 'T', foundationChar = 'F', stockChar = 'S';
        /// <summary>
        /// A list of all the previous state, so we don't end up in an infinite loop
        /// </summary>
        public List<State> stateHistory;

        /// <summary>
        /// Makes a list of every possible future state, paired with its score, from the current state
        /// </summary>
        /// <returns>List of KeyValuePairs, where Key=score and Value=State</returns>
        public List<KeyValuePair<int, State>> GetMoves()
        {
            List<KeyValuePair<int, State>> possibleMoves = new List<KeyValuePair<int, State>>();

            // Stock
            int cardTurnOverAmount = 3;
            for (int stockIndex = 0; stockIndex < stock.Count(); stockIndex++)
            {
                // Turn over 3 cards at a time (if you're not at the end of the stock)
                for(int turnOverIndex = 0; turnOverIndex < Math.Max(cardTurnOverAmount, stock.Count()); turnOverIndex++)
                {
                    waste.Push(stock.Pop());
                }
                possibleMoves.AddRange(AddMoves(waste.Peek().TopCard(), stockChar, -1));
            }

            // Tablueau
            for(int tableauIndex = 0; tableauIndex < tableaus.Count(); tableauIndex++)
            {
                foreach(Card card in GetTableauCards(tableaus[tableauIndex].Peek()))
                    possibleMoves.AddRange(AddMoves(card, tableauChar, tableauIndex));
            }

            // Foundation
            for(int foundationIndex = 0; foundationIndex < foundations.Count(); foundationIndex++)
            {
                possibleMoves.AddRange(AddMoves(foundations[foundationIndex].Peek().TopCard(), foundationChar, foundationIndex));
            }

            return possibleMoves;
        }

        /// <summary>
        /// Returns a list of every card you can see on top of a tableau
        /// </summary>
        /// <param name="topCard"></param>
        /// <returns></returns>
        private List<Card> GetTableauCards(Card topCard)
        {
            if (topCard.attached == null)
                return new List<Card> { topCard };
            else
            {
                List<Card> tableauCards = new List<Card>{ topCard };
                tableauCards.AddRange(GetTableauCards(topCard.attached));
                return tableauCards;
            }
        }

        /// <summary>
        /// Makes a list of every possible move you can do with a certain card
        /// </summary>
        /// <param name="card">The card you want to move</param>
        /// <param name="originChar">A character representing the type of CardStack the card came from (Tableau, Foundation or Stock)</param>
        /// <param name="originIndex">The index of the CardStack the card came from (-1 for the stock)</param>
        /// <returns></returns>
        private List<KeyValuePair<int, State>> AddMoves(Card card, char originChar, int originIndex)
        {
            List<KeyValuePair<int, State>> result = new List<KeyValuePair<int, State>>();

            for(int tableauIndex = 0; tableauIndex < tableaus.Count(); tableauIndex++)
            {
                if (IsMovePossible(card, tableaus[tableauIndex], tableauChar))
                {
                    KeyValuePair<int, State> move = AddMove(originChar, tableauChar, originIndex, tableauIndex);
                    if(!stateHistory.Contains(move.Value))
                        result.Add(move);
                }
            }
            // If the card we want to move is from a foundation already, we don't need to check if we can move it to a(n other) foundation
            if (!(originChar == foundationChar))
            {
                for (int foundationIndex = 0; foundationIndex < foundations.Count(); foundationIndex++)
                {
                    if (IsMovePossible(card, foundations[foundationIndex], foundationChar))
                    {
                        KeyValuePair<int, State> move = AddMove(originChar, foundationChar, originIndex, foundationIndex);
                        if(!stateHistory.Contains(move.Value))
                            result.Add(move);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if a move is possible, given a card, a CardStack and the type of the CardStack
        /// </summary>
        /// <param name="top">The Card you want to move to the CardStack</param>
        /// <param name="bottom">The CardStack you want to place a card on</param>
        /// <param name="destination">A character representing the type of CardStack (Tableau or Foundation)</param>
        /// <returns></returns>
        private bool IsMovePossible(Card top, CardStack bottom, char destination)
        {
            switch (destination)
            {
                // Move it on a tableau
                case tableauChar:
                    if (bottom.IsEmpty())
                        return top.rank == King;
                    Card bottomCard = bottom.Peek().TopCard();
                    return (top.rank == bottomCard.rank - 1) && 
                        ((top.suit == Hearts || top.suit == Diamonds) && (bottomCard.suit == Spades || bottomCard.suit == Clubs)) || 
                        ((top.suit == Spades || top.suit == Clubs)    && (bottomCard.suit == Hearts || bottomCard.suit == Diamonds));
                // Move it on a foundation
                case foundationChar:
                    if (bottom.IsEmpty())
                        return top.rank == Ace;
                    return (top.suit == bottom.Peek().TopCard().suit && top.rank == bottom.Peek().TopCard().rank + 1);

                default: throw new Exception("Destination incorrect");
            }
        }

        /// <summary>
        /// Make a new State (paired with its score,) from a known move, represented as characters and integers
        /// </summary>
        /// <param name="origin">A character representing the type of the origin CardStack</param>
        /// <param name="destination">A character representing the type of the destination CardStack</param>
        /// <param name="originIndex">An integer representing the index of the origin CardStack (-1 for the stock)</param>
        /// <param name="destinationIndex">An integer representing the index of the destination CardStack</param>
        /// <returns></returns>
        private KeyValuePair<int, State> AddMove(char origin, char destination, int originIndex, int destinationIndex)
        {
            State nextState = this;
            Card card;
            switch (origin)
            {
                // Tableau
                case tableauChar:
                    card = nextState.tableaus[originIndex].Pop();
                    break;
                // Foundation
                case foundationChar:
                    card = nextState.foundations[originIndex].Pop();
                    break;
                // Stock
                case stockChar:
                    card = nextState.waste.Pop();
                    // Move all cards from the waste back to the stock
                    for(int wasteIndex = 0; wasteIndex < nextState.stock.Count(); wasteIndex++)
                        nextState.stock.Push(nextState.waste.Pop());
                    break;

                default: throw new Exception("Origin incorrect");
            }
            switch (destination)
            {
                // Tableau
                case tableauChar:
                    nextState.tableaus[destinationIndex].Peek().TopCard().attached = card;
                    break;
                // Foundation
                case foundationChar:
                    nextState.foundations[destinationIndex].Push(card);
                    break;

                default: throw new Exception("Destination incorrect");

            }
            return new KeyValuePair<int, State> ( HeuristicFunction(nextState), nextState );
        }

        /// <summary>
        /// Function that gives a score to a future state
        /// </summary>
        /// <param name="nextState">The future state</param>
        /// <returns></returns>
        public int HeuristicFunction(State nextState)
        {
            return 0;
        }

        /*
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

            int card_turn_over_amount = 3;

            while(!stock.IsEmpty())
            {
                for (int stockIndex = 0; stockIndex < Math.Max(card_turn_over_amount, stock.Count()); stockIndex++)
                {
                    waste.Push(stock.Pop());
                }
                Card card = waste.Peek();
                foreach (int index in FindTableaus(card))
                {
                    State nextState = this;
                    nextState.tableaus[index].Push(nextState.waste.Pop());
                    for (int i = 0; i < nextState.stock.Count(); i++)
                    {
                        waste.Push(stock.Pop());
                    }
                    nextState.stock = nextState.waste;
                    nextState.waste = new CardStack();
                    // if(!stateHistory.Contains(nextState))
                    nextStates.Add(nextState);
                }
                int foundationIndex = FindFoundation(card);
                if (foundationIndex != -1)
                {
                    State nextState = this;
                    nextState.foundations[foundationIndex].Push(nextState.waste.Pop());
                    for (int i = 0; i < nextState.stock.Count(); i++)
                    {
                        waste.Push(stock.Pop());
                    }
                    nextState.stock = nextState.waste;
                    nextState.waste = new CardStack();
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

        public State AddMove()
        {
            return new State();
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
        */
    }
}
