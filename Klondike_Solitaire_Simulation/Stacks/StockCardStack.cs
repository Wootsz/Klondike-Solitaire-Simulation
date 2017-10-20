namespace Klondike_Solitaire_Simulation.Stacks
{
	public class StockCardStack : CardStack
	{
		/// <summary>
		/// The waste stack.
		/// </summary>
		public WasteCardStack Waste;

		/// <summary>
		/// The amount of Cards to move to the waste each time.
		/// </summary>
		public int MoveAmount;

		/// <inheritdoc />
		/// <summary>
		/// Creates a new stock stack.
		/// </summary>
		/// <param name="moveAmount">The amount of Cards to move to the waste each time.</param>
		public StockCardStack(int moveAmount)
		{
			Waste = new WasteCardStack(this);
			MoveAmount = moveAmount;
		}

		/// <inheritdoc />
		/// <summary>
		/// Copies the stock stack.
		/// </summary>
		/// <param name="original">The original stock stack.</param>
		public StockCardStack(StockCardStack original) : base(original)
		{
			Waste = new WasteCardStack(original.Waste)
			{
				Stock = this
			};
			MoveAmount = original.MoveAmount;
		}

		/// <summary>
		/// Moves the next Cards to the waste.
		/// </summary>
		public StockCardStack MoveToWaste()
		{
			MoveCardsFromTop(Waste, MoveAmount <= CardCount ? MoveAmount : CardCount, true);

			return this;
		}

		public override bool CanPlaceCardOnTop(Card card) => false;

		public override bool CanRemoveCardFromTop() => false;
	}
}