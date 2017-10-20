using System.Collections.Generic;

namespace Klondike_Solitaire_Simulation.Stacks
{
	public class WasteCardStack : CardStack
	{
		public override List<Card> MovableCards => new List<Card>()
		{
			TopCard
		};

		/// <summary>
		/// The stock stack.
		/// </summary>
		public StockCardStack Stock;

		/// <inheritdoc />
		/// <summary>
		/// Creates a new waste stack.
		/// </summary>
		/// <param name="stock">The stock stack.</param>
		public WasteCardStack(StockCardStack stock) => Stock = stock;

		/// <inheritdoc />
		/// <summary>
		/// Copies the waste stack.
		/// </summary>
		/// <param name="original">The original waste stack.</param>
		public WasteCardStack(CardStack original) : base(original)
		{
		}

		public override bool CanPlaceCardOnTop(Card card) => false;

		/// <summary>
		/// Empties the waste back into the stock.
		/// </summary>
		public WasteCardStack Empty()
		{
			MoveCardsFromTop(Stock, Cards.Count, true);

			return this;
		}
	}
}