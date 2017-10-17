using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klondike_Solitaire_Simulation.Stacks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
	public class State
	{
		/// <summary>
		/// The cards left in the stock.
		/// </summary>
		public StockStack stock = new StockStack();

		/// <summary>
		/// The current waste (cards that can be moved to tableaus).
		/// </summary>
		public WasteStack waste = new WasteStack();

		// Heuristic Modifiers
		public int foundationModifier = 1;
		public int tableauModifier = 1;
		public int wastModifier = 0;
		public int stockModifier = 0;

		/// <summary>
		/// The foundations where the final piles can be placed.
		/// </summary>
		public List<FoundationCardStack> foundations = new List<FoundationCardStack>() {
			new FoundationCardStack(),
			new FoundationCardStack(),
			new FoundationCardStack(),
			new FoundationCardStack()
		};

		/// <summary>
		/// The actual game stacks where the cards are moved to and from.
		/// </summary>
		public List<TableauCardStack> tableaus = new List<TableauCardStack>() {
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack()
		};

		/// <summary>
		/// Creates card new game state.
		/// </summary>
		public State()
		{
			// Generate default stack and shuffle
			CardStack deck = CardStack.GenerateStandardDeck();
			deck.Shuffle();

			// Shuffle the deck
			deck.Shuffle();

			// Fill tableaus
			for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
			{
				// Add cards from the deck
				deck.MoveFromTop(tableaus[tableauIndex], tableauIndex + 1, true);

				// Flip the topmost card
				tableaus[tableauIndex].Peek().Flip();
			}

			// Move the rest to the stock
			stock.Push(deck);
			stock.FlipAll();
		}

		/// <summary>
		/// Gets card string representation of the current state.
		/// </summary>
		/// <returns>The current state as card string.</returns>
		public override string ToString()
		{
			string result = "";

			// Output stacks
			result += "Stock: " + stock + "\n";

			result += "Waste: " + waste + "\n";

			foreach (FoundationCardStack foundation in foundations)
			{
				result += "Foundation: " + foundation + "\n";
			}

			foreach (TableauCardStack tableau in tableaus)
			{
				result += "Tableau: " + tableau + "\n";
			}

			result += "\n";

			return result;
		}

		const char tableauChar = 'T', foundationChar = 'F', stockChar = 'S';
		/// <summary>
		/// A list of all the previous state, so we don't end up in an infinite loop
		/// </summary>
		public List<State> stateHistory = new List<State>();

		/// <summary>
		/// Makes a list of every possible future state, paired with its score, from the current state
		/// </summary>
		/// <returns>List of KeyValuePairs, where Key=score and Value=State</returns>
		public List<KeyValuePair<int, State>> GetMoves()
		{
			List<KeyValuePair<int, State>> possibleMoves = new List<KeyValuePair<int, State>>();

			// Stock
			int cardTurnOverAmount = 3;
			for (int stockIndex = 0; stockIndex < stock.Count(); stockIndex++)
			{
				// Turn over 3 cards at a time (if you're not at the end of the stock)
				for (int turnOverIndex = 0; turnOverIndex < Math.Min(cardTurnOverAmount, stock.Count()); turnOverIndex++)
				{
					waste.Push(stock.Pop());
				}
				possibleMoves.AddRange(AddMoves(waste.Peek().TopCard(), stockChar, -1));
			}

			// Tablueau
			for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
			{
				foreach (Card card in GetTableauCards(tableaus[tableauIndex].Peek()))
				{
					possibleMoves.AddRange(AddMoves(card, tableauChar, tableauIndex));
				}
			}

			// Foundation
			for (int foundationIndex = 0; foundationIndex < foundations.Count; foundationIndex++)
			{
				possibleMoves.AddRange(AddMoves(foundations[foundationIndex].Peek().TopCard(), foundationChar, foundationIndex));
			}

			return possibleMoves;
		}

		/// <summary>
		/// Returns a list of every card you can see on top of a tableau.
		/// </summary>
		/// <param name="topCard"></param>
		/// <returns></returns>
		private List<Card> GetTableauCards(Card topCard)
		{
			if (topCard.attached == null)
			{
				return new List<Card> {
					topCard
				};
			}
			else
			{
				List<Card> tableauCards = new List<Card> {
					topCard
				};
				tableauCards.AddRange(GetTableauCards(topCard.attached));
				return tableauCards;
			}
		}

		/// <summary>
		/// Makes a list of every possible move you can do with a certain card.
		/// </summary>
		/// <param name="card">The card you want to move</param>
		/// <param name="originChar">A character representing the type of CardStack the card came from (Tableau, Foundation or Stock)</param>
		/// <param name="originIndex">The index of the CardStack the card came from (-1 for the stock)</param>
		/// <returns></returns>
		private List<KeyValuePair<int, State>> AddMoves(Card card, char originChar, int originIndex)
		{
			List<KeyValuePair<int, State>> result = new List<KeyValuePair<int, State>>();

			for (int tableauIndex = 0; tableauIndex < tableaus.Count; tableauIndex++)
			{
				if (tableaus[tableauIndex].IsMovePossible(card))
				{
					KeyValuePair<int, State> move = AddMove(originChar, tableauChar, originIndex, tableauIndex);

					if (!stateHistory.Contains(move.Value))
					{
						result.Add(move);
					}
				}
			}

			// If the card we want to move is from a foundation already, we don't need to check if we can move it to a(n other) foundation
			if (!(originChar == foundationChar))
			{
				for (int foundationIndex = 0; foundationIndex < foundations.Count; foundationIndex++)
				{
					if (foundations[foundationIndex].IsMovePossible(card))
					{
						KeyValuePair<int, State> move = AddMove(originChar, foundationChar, originIndex, foundationIndex);

						if (!stateHistory.Contains(move.Value))
						{
							result.Add(move);
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Make a new State (paired with its score,) from a known move, represented as characters and integers
		/// </summary>
		/// <param name="origin">A character representing the type of the origin CardStack</param>
		/// <param name="destination">A character representing the type of the destination CardStack</param>
		/// <param name="originIndex">An integer representing the index of the origin CardStack (-1 for the stock)</param>
		/// <param name="destinationIndex">An integer representing the index of the destination CardStack</param>
		/// <returns></returns>
		private KeyValuePair<int, State> AddMove(char origin, char destination, int originIndex, int destinationIndex)
		{
			State nextState = this;
			Card card;
			switch (origin)
			{
				// Tableau
				case tableauChar:
					card = nextState.tableaus[originIndex].Pop();
					break;
				// Foundation
				case foundationChar:
					card = nextState.foundations[originIndex].Pop();
					break;
				// Stock
				case stockChar:
					card = nextState.waste.Pop();
					// Move all cards from the waste back to the stock
					int wasteLength = nextState.waste.Count();
					for (int wasteIndex = 0; wasteIndex < wasteLength; wasteIndex++)
						nextState.stock.Push(nextState.waste.Pop());
					break;

				default:
					throw new Exception("Origin incorrect");
			}

			switch (destination)
			{
				// Tableau
				case tableauChar:
					nextState.tableaus[destinationIndex].Peek().TopCard().attached = card;

					break;

				// Foundation
				case foundationChar:
					nextState.foundations[destinationIndex].Push(card);

					break;

				default:
					throw new Exception("Destination incorrect");
			}

			return new KeyValuePair<int, State>(HeuristicFunction(nextState), nextState);
		}

		/// <summary>
		/// Function that gives a score to a future state
		/// </summary>
		/// <param name="nextState">The future state</param>
		/// <returns></returns>
		public int HeuristicFunction(State nextState)
		{
			int totalScore = 0;

			//Stock score
			totalScore += (nextState.stock.Count() * stockModifier);

			//Waste score
			totalScore += (nextState.waste.Count() * wastModifier);

			//Tableau score
			foreach (CardStack tableau in nextState.tableaus)
			{
				if (tableau.Count() > 0)
				{
					Card card = tableau.Peek();
					int localScore = 1;
					while (true)
					{
						if (card.attached != null)
						{
							card = card.attached;
							localScore++;
							continue;
						}
						break;
					}

					totalScore += (localScore * foundationModifier);

				}

			}

			//Foundation score
			foreach (CardStack foundation in nextState.foundations)
			{
				totalScore += (foundation.Count() * foundationModifier);
			}


			Console.WriteLine(totalScore);
			return totalScore;
		}

	}
}
