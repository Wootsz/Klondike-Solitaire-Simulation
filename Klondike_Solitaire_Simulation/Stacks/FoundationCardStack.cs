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
		/// Checks if a move is possible given a card.
		/// </summary>
		/// <param name="card">The Card you want to move to the stack.</param>
		/// <returns></returns>
		public bool IsMovePossible(Card card)
		{
			if (IsEmpty())
			{
				return card.rank == Rank.Ace;
			}

			return card.suit == Peek().TopCard().suit && card.rank == Peek().TopCard().rank + 1;
		}
	}
}
