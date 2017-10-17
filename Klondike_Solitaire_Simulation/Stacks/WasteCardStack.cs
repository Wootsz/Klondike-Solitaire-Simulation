using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Stacks
{
	public class WasteCardStack : CardStack
	{
		/// <summary>
		/// The stock stack.
		/// </summary>
		public StockCardStack Stock;

		/// <summary>
		/// Creates a new waste stack.
		/// </summary>
		/// <param name="stock">The stock stack.</param>
		public WasteCardStack(StockCardStack stock)
		{
			Stock = stock;
		}

		/// <summary>
		/// Copies the waste stack.
		/// </summary>
		/// <param name="original">The original waste stack.</param>
		public WasteCardStack(WasteCardStack original) : base(original)
		{
		}

		public override bool CanPlaceCardOnTop(Card card)
		{
			return false;
		}

		/// <summary>
		/// Empties the waste back into the stock.
		/// </summary>
		public WasteCardStack Empty()
		{
			MoveCardsFromTop(Stock, cards.Count, true);

			return this;
		}
	}
}
