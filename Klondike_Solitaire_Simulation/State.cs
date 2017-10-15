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
		/// The current deck.
		/// </summary>
		public CardStack deck = CardStack.GenerateStandardDeck();

		/// <summary>
		/// The cards left in the stock.
		/// </summary>
		public CardStack stock = new CardStack();

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
			// Shuffle the deck
			deck.Shuffle();

			// Fill tableaus
			for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
			{
				deck.MoveFromTop(tableaus[tableauIndex], tableauIndex + 1);
			}

			// Whatever's left, goes into the stock
			stock = deck;
		}

		/// <summary>
		/// Check if you can place card card on b
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public bool IsValidMove(Card top, Card bottom)
		{
			if (top.rank == bottom.rank - 1)
			{
				if (top.suit == Diamonds || bottom.suit == Hearts)
				{
					return (bottom.suit == Clubs || bottom.suit == Spades);
				}
				else
				{
					return (bottom.suit == Diamonds || bottom.suit == Hearts);
				}
			}

			return false;
		}

		/// <summary>
		/// Check if you can place card card on top of card foundation
		/// </summary>
		/// <param name="card"></param>
		/// <returns>The index of the foundation you can place card card on</returns>
		public int FindFoundation(Card card)
		{
			for (int foundationIndex = 0; foundationIndex < foundations.Count; foundationIndex++) {
				if (foundations[foundationIndex].IsEmpty() && card.rank == A || card.suit == foundations[foundationIndex].Peek().suit && card.rank == foundations[foundationIndex].Peek().rank - 1)
				{
					return foundationIndex;
				}
			}

			return -1;
		}

		/// <summary>
		/// Gets card string representation of the current state.
		/// </summary>
		/// <returns>The current state as card string.</returns>
		public override string ToString()
		{
			string output = "";

			// Output deck
			output += "Deck: " + String.Join(", ", deck);

			return output;
		}
	}
}
