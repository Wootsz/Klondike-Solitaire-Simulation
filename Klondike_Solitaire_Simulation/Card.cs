using System;
using static Klondike_Solitaire_Simulation.Suit;

namespace Klondike_Solitaire_Simulation
{
	public enum Suit
	{
		Spades,
		Hearts,
		Clubs,
		Diamonds
	}

	public enum Color
	{
		Black,
		Red
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
		/// Whether the card is flipped (invisible).
		/// </summary>
		public bool Flipped;

		/// <summary>
		/// The rank of the card.
		/// </summary>
		public Rank Rank;

		/// <summary>
		/// The suit of the card.
		/// </summary>
		public Suit Suit;

		/// <summary>
		/// Creates a new card.
		/// </summary>
		/// <param name="suit">The suit.</param>
		/// <param name="rank">The rank.</param>
		public Card(Suit suit, Rank rank)
		{
			Suit = suit;
			Rank = rank;
		}

		/// <summary>
		/// Copies a card.
		/// </summary>
		/// <param name="original">The card to copy.</param>
		public Card(Card original)
		{
			Suit = (Suit) Enum.Parse(typeof(Suit), original.Suit.ToString());
			Rank = (Rank) Enum.Parse(typeof(Rank), original.Rank.ToString());
			Flipped = original.Flipped;
		}

		/// <summary>
		/// The color of the current suit.
		/// </summary>
		public Color Color => Suit == Spades || Suit == Clubs ? Color.Black : Color.Red;

		/// <summary>
		/// Flips the card.
		/// </summary>
		public Card Flip()
		{
			Flipped = !Flipped;

			return this;
		}

		/// <summary>
		/// Converts the Card to a string representation.
		/// </summary>
		/// <returns>A string representation of the card.</returns>
		public override string ToString()
		{
			int rankNumber = (int) Rank + 1;
			string rankLetter = Rank.ToString()[0].ToString();
			string suitLetter = "";
			switch (Suit)
			{
				case Clubs:
					suitLetter = "♣";
					break;

				case Diamonds:
					suitLetter = "♦";
					break;

				case Hearts:
					suitLetter = "♥";
					break;

				case Spades:
					suitLetter = "♠";
					break;
			}

			return (Flipped ? "[" : "") + (rankNumber == 1 || rankNumber > 10 ? rankLetter : rankNumber.ToString()) + suitLetter + (Flipped ? "]" : "");
		}
	}
}