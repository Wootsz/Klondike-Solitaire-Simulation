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
		/// Creates a new tableau stack.
		/// </summary>
		public TableauCardStack()
		{
		}

		/// <summary>
		/// Copies the tableau stack.
		/// </summary>
		/// <param name="original">The original tablue stack.</param>
		public TableauCardStack(TableauCardStack original) : base(original)
		{
		}

		/// <summary>
		/// Checks if a move is possible given a card.
		/// </summary>
		/// <param name="card">The Card you want to move to the stack.</param>
		/// <returns></returns>
		public override bool CanPlaceCardOnTop(Card card)
		{
			// First check if the tableau is empty
			if (IsEmpty())
			{
				// If it's empty, the next card needs to be a king
				return card.Rank == Rank.King;
			}
			else if (IsFull())
			{
				return false;
			}
			else
			{
				// Otherwise, check the rules
				bool isLowerRank = card.Rank == PeekAtTopCard().Rank - 1;
				bool isAlternateColor = card.Color != PeekAtTopCard().Color;
				bool isSpace = PeekAtTopCard().Rank != Rank.Ace;

				return isLowerRank && isAlternateColor && isSpace;
			}
		}
	}
}
