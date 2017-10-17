using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Stacks
{
	public class FoundationCardStack : CardStack
	{
		/// <summary>
		/// Creates a new foundation stack.
		/// </summary>
		public FoundationCardStack() : base(13)
		{
		}

		/// <summary>
		/// Copies the foundation stack.
		/// </summary>
		/// <param name="original">The original foundation stack.</param>
		public FoundationCardStack(FoundationCardStack original) : base(original)
		{
		}

		/// <summary>
		/// Checks if a move is possible given a card.
		/// </summary>
		/// <param name="card">The Card you want to move to the stack.</param>
		/// <returns></returns>
		public override bool CanPlaceCardOnTop(Card card)
		{
			// First check if the foundation is empty
			if (IsEmpty())
			{
				// If it's empty, the card only needs to be an ace
				return card.Rank == Rank.Ace;
			}
			else if (IsFull())
			{
				return false;
			}
			else
			{
				// Check if the card can be placed
				bool isSameSuit = card.Suit == PeekAtTopCard().Suit;
				bool isNextRank = card.Rank == PeekAtTopCard().Rank + 1;

				return isSameSuit && isNextRank;
			}
		}
	}
}
