using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
	public enum Suit
	{
		Spades,
		Hearts,
		Clubs,
		Diamonds
	}

	public enum Rank
	{
		Ace,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King
	}

	public class Card
	{
		/// <summary>
		/// The suit of the card.
		/// </summary>
		public Suit suit;

		/// <summary>
		/// The rank of the card.
		/// </summary>
		public Rank rank;

		/// <summary>
		/// Whether the card is flipped (invisible).
		/// </summary>
		public bool flipped = false;

		/// <summary>
		/// The card that's attached.
		/// </summary>
		public Card attached;

		public Card(Suit suit, Rank rank)
		{
			this.suit = suit;
			this.rank = rank;
		}

		public Card TopCard()
		{
			if (attached == null)
			{
				return this;
			}
			else
			{
				return attached.TopCard();
			}
		}

		/// <summary>
		/// Flips the card.
		/// </summary>
		public void Flip()
		{
			flipped = !flipped;
		}

		public override string ToString() => rank + " of " + suit + " (" + (flipped ? "flipped" : "normal") + ")";
	}
}
