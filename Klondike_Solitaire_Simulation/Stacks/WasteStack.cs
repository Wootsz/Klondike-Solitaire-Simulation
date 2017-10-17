using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Stacks
{
	public class WasteStack : CardStack
	{
		/// <summary>
		/// Empties the waste back into the stock.
		/// </summary>
		/// <param name="stock">The stock to empty into.</param>
		public void Empty(StockStack stock) {
			MoveFromTop(stock, cards.Count, true);
		}
	}
}
