namespace Klondike_Solitaire_Simulation.Stacks
{
	public class FoundationCardStack : CardStack
	{
		/// <inheritdoc />
		/// <summary>
		/// Creates a new foundation stack.
		/// </summary>
		public FoundationCardStack()
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// Copies the foundation stack.
		/// </summary>
		/// <param name="original">The original foundation stack.</param>
		public FoundationCardStack(FoundationCardStack original) : base(original)
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// Checks if a move is possible given a card.
		/// </summary>
		/// <param name="card">The Card you want to move to the stack.</param>
		/// <returns></returns>
		public override bool CanPlaceCardOnTop(Card card)
		{
			// First check if the foundation is empty
			if (IsEmpty)
			{
				// If it's empty, the card only needs to be an ace
				return card.Rank == Rank.Ace;
			}
			else if (IsFull)
			{
				return false;
			}
			else
			{
				// Check if the card can be placed
				bool isSameSuit = card.Suit == TopCard.Suit;
				bool isNextRank = card.Rank == TopCard.Rank + 1;
				bool isSpace = TopCard.Rank != Rank.King;

				return isSameSuit && isNextRank && isSpace;
			}
		}
	}
}