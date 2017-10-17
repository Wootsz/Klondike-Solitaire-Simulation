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
		// Settings
		public const int WasteCardAmount = 3;

		/// <summary>
		/// The heuristics score of the state.
		/// </summary>
		public int Score
		{
			get
			{
				// Heuristic Modifiers
				const int FoundationModifier = 1;
				const int TableauModifier = 1;
				const int WasteModifier = 1;
				const int StockModifier = 1;

				int totalScore = 0;

				// Stock score
				totalScore += Stock.CardCount * StockModifier;

				// Waste score
				totalScore += Stock.Waste.CardCount * WasteModifier;

				// Tableau score
				foreach (CardStack tableau in Tableaus)
				{
					totalScore += tableau.CardCount * TableauModifier;
				}

				// Foundation score
				foreach (CardStack foundation in Foundations)
				{
					totalScore += foundation.CardCount * FoundationModifier;
				}


				Console.WriteLine(totalScore);
				return totalScore;
			}
		}

		/// <summary>
		/// The cards left in the stock.
		/// </summary>
		public StockCardStack Stock = new StockCardStack(WasteCardAmount);

		/// <summary>
		/// The foundations where the final piles can be placed.
		/// </summary>
		public List<FoundationCardStack> Foundations = new List<FoundationCardStack>() {
			new FoundationCardStack(),
			new FoundationCardStack(),
			new FoundationCardStack(),
			new FoundationCardStack()
		};

		/// <summary>
		/// The actual game stacks where the cards are moved to and from.
		/// </summary>
		public List<TableauCardStack> Tableaus = new List<TableauCardStack>() {
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack(),
			new TableauCardStack()
		};

		/// <summary>
		/// All card stacks.
		/// </summary>
		public List<CardStack> CardStacks
		{
			get
			{
				List<CardStack> result = new List<CardStack>() {
					Stock,
					Stock.Waste
				};
				result.AddRange(Foundations);
				result.AddRange(Tableaus);

				return result;
			}
		}

		/// <summary>
		/// Creates card new game state.
		/// </summary>
		public State()
		{
			// Generate default stack and shuffle
			CardStack deck = CardStack.GenerateStandardDeck();

			// Shuffle the deck
			deck.ShuffleCards();

			// Fill tableaus
			for (int tableauIndex = 0; tableauIndex < Tableaus.Count; tableauIndex++)
			{
				// Add cards from the deck
				deck.MoveCardsFromTop(Tableaus[tableauIndex], tableauIndex + 1, true);

				// Flip the topmost card
				Tableaus[tableauIndex].PeekAtTopCard().Flip();
			}

			// Move the rest to the stock
			Stock.AddStackToTop(deck);
			Stock.FlipAllCards();
		}

		/// <summary>
		/// Copies the old state into a new object.
		/// </summary>
		/// <param name="original">The state to copy.</param>
		public State(State original)
		{
			// Copy stock
			Stock = new StockCardStack(original.Stock);

			// Copy foundations
			for (int foundationIndex = 0; foundationIndex < original.Foundations.Count; ++foundationIndex)
			{
				Foundations[foundationIndex] = new FoundationCardStack(original.Foundations[foundationIndex]);
			}

			// Copy tableaus
			for (int tableauIndex = 0; tableauIndex < original.Tableaus.Count; ++tableauIndex)
			{
				Tableaus[tableauIndex] = new TableauCardStack(original.Tableaus[tableauIndex]);
			}
		}

		/// <summary>
		/// Gets card string representation of the current state.
		/// </summary>
		/// <returns>The current state as card string.</returns>
		public override string ToString()
		{
			return ToString(false, false);
		}

		public string ToString(bool printMoves, bool recursive = false)
		{
			string result = "";

			// Output stock and waste
			result += "Stock: " + Stock;

			// Output foundations
			foreach (FoundationCardStack foundation in Foundations)
			{
				result += "\n" + "Foundation: " + foundation;
			}

			// Output tableaus
			foreach (TableauCardStack tableau in Tableaus)
			{
				result += "\n" + "Tableau: " + tableau;
			}

			if (printMoves)
			{
				List<State> moves = GetMoves();

				result += "\n";
				result += "\nAmount of possible moves: " + moves.Count;

				for (int moveIndex = 0; moveIndex < moves.Count; ++moveIndex)
				{
					State currentState = moves[moveIndex];

					result += "\n\n[#" + (moveIndex + 1) + "]";

					result += "\n" + currentState.ToString(recursive, recursive);
				}
			}

			return result;
		}

		/// <summary>
		/// Makes a list of every possible future state, paired with its score, from the current state
		/// </summary>
		/// <returns>List of KeyValuePairs, where Key=score and Value=State</returns>
		public List<State> GetMoves()
		{
			List<State> result = new List<State>();

			// State where next cards are moved to the waste
			State stockToWaste = new State(this);
			stockToWaste.Stock.MoveToWaste();
			result.Add(stockToWaste);

			// All possible card movements
			foreach (CardStack sourceStack in CardStacks)
			{
				if (!sourceStack.IsEmpty() && sourceStack.CanRemoveCardFromTop())
				{
					foreach (CardStack targetStack in CardStacks)
					{
						if (sourceStack != targetStack && targetStack.CanPlaceCardOnTop(sourceStack.PeekAtTopCard()))
						{
							// Clone state
							State newState = new State(this);

							// Make move in new state
							newState.CardStacks[CardStacks.IndexOf(sourceStack)].MoveCardsFromTop(newState.CardStacks[CardStacks.IndexOf(targetStack)], 1, false, true);

							// Add new state
							result.Add(newState);
						}
					}
				}
			}

			return result;
		}

		/*
		const char tableauChar = 'T', foundationChar = 'F', stockChar = 'S';
		/// <summary>
		/// A list of all the previous state, so we don't end up in an infinite loop
		/// </summary>
		public List<State> stateHistory = new List<State>();

		public List<KeyValuePair<int, State>> GetMoves()
		{
			List<KeyValuePair<int, State>> possibleMoves = new List<KeyValuePair<int, State>>();

			// Stock
			int cardTurnOverAmount = 3;
			for (int stockIndex = 0; stockIndex < Stock.CardCount; stockIndex++)
			{
				// Turn over 3 cards at a time (if you're not at the end of the stock)
				for (int turnOverIndex = 0; turnOverIndex < Math.Min(cardTurnOverAmount, Stock.CardCount); turnOverIndex++)
				{
					Waste.AddCardToTop(Stock.RemoveTopCard());
				}
				possibleMoves.AddRange(AddMoves(Waste.PeekAtTopCard().TopCard(), stockChar, -1));
			}

			// Tablueau
			for (int tableauIndex = 0; tableauIndex < Tableaus.Count; tableauIndex++)
			{
				foreach (Card card in GetTableauCards(Tableaus[tableauIndex].PeekAtTopCard()))
				{
					possibleMoves.AddRange(AddMoves(card, tableauChar, tableauIndex));
				}
			}

			// Foundation
			for (int foundationIndex = 0; foundationIndex < Foundations.Count; foundationIndex++)
			{
				possibleMoves.AddRange(AddMoves(Foundations[foundationIndex].PeekAtTopCard().TopCard(), foundationChar, foundationIndex));
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
			if (topCard.AttachedCard == null)
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
				tableauCards.AddRange(GetTableauCards(topCard.AttachedCard));
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

			for (int tableauIndex = 0; tableauIndex < Tableaus.Count; tableauIndex++)
			{
				if (Tableaus[tableauIndex].IsMovePossible(card))
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
				for (int foundationIndex = 0; foundationIndex < Foundations.Count; foundationIndex++)
				{
					if (Foundations[foundationIndex].IsMovePossible(card))
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
					card = nextState.Tableaus[originIndex].RemoveTopCard();
					break;
				// Foundation
				case foundationChar:
					card = nextState.Foundations[originIndex].RemoveTopCard();
					break;
				// Stock
				case stockChar:
					card = nextState.Waste.RemoveTopCard();
					// Move all cards from the waste back to the stock
					int wasteLength = nextState.Waste.CardCount;
					for (int wasteIndex = 0; wasteIndex < wasteLength; wasteIndex++)
						nextState.Stock.AddCardToTop(nextState.Waste.RemoveTopCard());
					break;

				default:
					throw new Exception("Origin incorrect");
			}

			switch (destination)
			{
				// Tableau
				case tableauChar:
					nextState.Tableaus[destinationIndex].PeekAtTopCard().TopCard().AttachedCard = card;

					break;

				// Foundation
				case foundationChar:
					nextState.Foundations[destinationIndex].AddCardToTop(card);

					break;

				default:
					throw new Exception("Destination incorrect");
			}

			return new KeyValuePair<int, State>(HeuristicFunction(nextState), nextState);
		}
		*/
	}
}
