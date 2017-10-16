using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Stacks
{
	public class TableauCardStack : CardStack
	{
		/// <summary>
		/// Checks if a move is possible given a card.
		/// </summary>
		/// <param name="card">The Card you want to move to the stack.</param>
		/// <returns></returns>
		public bool IsMovePossible(Card card)
		{
			if (IsEmpty()) {
				return card.rank == Rank.King;
			}

			Card bottomCard = Peek().TopCard();

			return card.rank == bottomCard.rank - 1 && (card.suit == Suit.Hearts || card.suit == Suit.Diamonds) && (bottomCard.suit == Suit.Spades || bottomCard.suit == Suit.Clubs) ||
				   (card.suit == Suit.Spades || card.suit == Suit.Clubs) && (bottomCard.suit == Suit.Hearts || bottomCard.suit == Suit.Diamonds);
		}
	}
}
