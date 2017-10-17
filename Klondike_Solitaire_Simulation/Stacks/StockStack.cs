using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Stacks
{
	public class StockStack : CardStack
	{
		/// <summary>
		/// Moves a specified amount of cards to the waste.
		/// </summary>
		/// <param name="waste">The waste.</param>
		/// <param name="amount">The amount of cards to move.</param>
		public void MoveToWaste(WasteStack waste, int amount) {
			MoveFromTop(waste, amount, true);
		}
	}
}
