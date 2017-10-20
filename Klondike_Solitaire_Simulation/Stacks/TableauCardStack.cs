namespace Klondike_Solitaire_Simulation.Stacks
{
	public class TableauCardStack : CardStack
	{
		/// <inheritdoc />
		/// <summary>
		/// Creates a new tableau stack.
		/// </summary>
		public TableauCardStack()
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// Copies the tableau stack.
		/// </summary>
		/// <param name="original">The original tablue stack.</param>
		public TableauCardStack(CardStack original) : base(original)
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
				bool isLowerRank = card.Rank == TopCard.Rank - 1;
				bool isAlternateColor = card.Color != TopCard.Color;
				bool isSpace = TopCard.Rank != Rank.Ace;

				return isLowerRank && isAlternateColor && isSpace;
			}
		}

		public override CardStack MoveCardsFromTop(CardStack otherStack, int amount, bool flip = false, bool reverse = true)
		{
			CardStack result = base.MoveCardsFromTop(otherStack, amount, flip, reverse);

			if (!IsEmpty() && TopCard.Flipped)
			{
				TopCard.Flip();
			}

			return result;
		}
	}
}