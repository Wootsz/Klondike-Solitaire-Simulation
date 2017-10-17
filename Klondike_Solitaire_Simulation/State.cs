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
				const int FoundationModifier = 2;
				const int TableauModifier = 1;
				const int WasteModifier = 0;
				const int StockModifier = 0;

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
			string result = "State score: " + Score;

			// Output stock and waste
			result += "\nStock: " + Stock;

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
		*/
	}
}
