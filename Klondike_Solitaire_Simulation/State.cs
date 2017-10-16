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
		public List<CardStack> foundations = new List<CardStack>() {
			new CardStack(),
			new CardStack(),
			new CardStack(),
			new CardStack()
		};

		/// <summary>
		/// The actual game stacks where the cards are moved to and from.
		/// </summary>
		public List<CardStack> tableaus = new List<CardStack>() {
			new CardStack(),
			new CardStack(),
			new CardStack(),
			new CardStack(),
			new CardStack(),
			new CardStack(),
			new CardStack()
		};

		/// <summary>
		/// Creates card new game state.
		/// </summary>
		public State()
		{
			// Shuffle the stock
			stock.Shuffle();

			// Fill tableaus
			for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
			{
				stock.MoveFromTop(tableaus[tableauIndex], tableauIndex + 1);
			}
		}

		/// <summary>
		/// Check if you can place the top card on the bottom card.
		/// </summary>
		/// <param name="top">The top card.</param>
		/// <param name="bottom">The bottom card.</param>
		/// <returns>Whether the move is possible.</returns>
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
		/// Check if you can place card card on top of card foundation.
		/// </summary>
		/// <param name="card">The card to check.</param>
		/// <returns>The index of the foundation you can place card card on.</returns>
		public int FindFoundation(Card card)
		{
			for (int foundationIndex = 0; foundationIndex < foundations.Count; foundationIndex++) {
				if (foundations[foundationIndex].IsEmpty() && card.rank == Ace || card.suit == foundations[foundationIndex].Peek().suit && card.rank == foundations[foundationIndex].Peek().rank - 1)
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
			string result = "";

			// Output stacks
			result += "Stock: " + stock.ToString() + "\n";

			result += "Waste: " + waste.ToString() + "\n";

			foreach (CardStack foundation in foundations) {
				result += "Foundation: " + foundation.ToString() + "\n";
			}

			foreach (CardStack tableau in tableaus) {
				result += "Tableau: " + tableau.ToString() + "\n";
			}

			result += "\n";

			return result;
		}
	}
}
