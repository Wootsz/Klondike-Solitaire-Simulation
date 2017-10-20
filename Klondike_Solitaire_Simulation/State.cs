using System;
using System.Collections.Generic;
using System.Linq;
using Klondike_Solitaire_Simulation.Stacks;

namespace Klondike_Solitaire_Simulation
{
	public class State
	{
		// Settings
		public const int WasteCardAmount = 3;

		/// <summary>
		/// The current number of this state.
		/// </summary>
		public int CurrentStateNumber = 1;

		/// <summary>
		/// The foundations where the final piles can be placed.
		/// </summary>
		public List<FoundationCardStack> Foundations = new List<FoundationCardStack>
		{
			new FoundationCardStack(),
			new FoundationCardStack(),
			new FoundationCardStack(),
			new FoundationCardStack()
		};

		/// <summary>
		/// The previous state.
		/// </summary>
		public State PreviousState;

		/// <summary>
		/// The Cards left in the stock.
		/// </summary>
		public StockCardStack Stock = new StockCardStack(WasteCardAmount);

		/// <summary>
		/// The actual game stacks where the Cards are moved to and from.
		/// </summary>
		public List<TableauCardStack> Tableaus = new List<TableauCardStack>
		{
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

			// Shuffle the deck
			deck.ShuffleCards();

			// Fill tableaus
			for (int tableauIndex = 0; tableauIndex < Tableaus.Count; tableauIndex++)
			{
				// Add Cards from the deck
				deck.MoveCardsFromTop(Tableaus[tableauIndex], tableauIndex + 1, true);

				// Flip the topmost card
				Tableaus[tableauIndex].TopCard.Flip();
			}

			// Move the rest to the stock
			Stock.AddStackToTop(deck);
			Stock.FlipAllCards();

			// Set up card stacks
			CardStacks = new List<CardStack>
			{
				Stock,
				Stock.Waste
			}.Concat(Foundations).Concat(Tableaus).ToList();
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

			CurrentStateNumber = original.CurrentStateNumber;

			// Set up card stacks
			CardStacks = new List<CardStack>
			{
				Stock,
				Stock.Waste
			}.Concat(Foundations).Concat(Tableaus).ToList();
		}

		/// <summary>
		/// The total number of this state.
		/// </summary>
		public List<int> TotalStateNumber
		{
			get
			{
				if (PreviousState == null)
				{
					return new List<int>
					{
						CurrentStateNumber
					};
				}
				else
				{
					List<int> result = PreviousState.TotalStateNumber;

					result.Add(CurrentStateNumber);

					return result;
				}
			}
		}

		/// <summary>
		/// Whether this state is a repeat of a previous state.
		/// </summary>
		public bool IsRepeatedState
		{
			get
			{
				State currentState = this;
				while ((currentState = currentState.PreviousState) != null)
				{
					if (IsSame(currentState))
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// All card stacks.
		/// </summary>
		public List<CardStack> CardStacks;

		/// <summary>
		/// Whether this state is an end state.
		/// </summary>
		public bool IsEndState => GetMoves().Count == 0;

		/// <summary>
		/// Whether this state is a win state.
		/// </summary>
		public bool IsWinState => Foundations.All(foundation => foundation.CardCount == 13);

		/// <summary>
		/// The amount of moves made to get to this state.
		/// </summary>
		public int MovesMade => TotalStateNumber.Count - 1;

		/// <summary>
		/// Gets card string representation of the current state.
		/// </summary>
		/// <returns>The current state as card string.</returns>
		public override string ToString() => ToString(false);

		public string ToString(bool printMoves, int recursionCount = 0, string indent = "|")
		{
			string result = indent + "- State #" + String.Join(".", TotalStateNumber);

			result += "\n" + indent + "  Is end state: " + IsEndState;

			result += "\n" + indent + "  Is win state: " + IsWinState;

			result += "\n" + indent + "  Moves before made so far: " + MovesMade;

			// Output stock and waste
			result += "\n" + indent + "  Stock: " + Stock;
			result += "\n" + indent + "  Waste: " + Stock.Waste;

			// Output foundations
			result = Foundations.Aggregate(result, (current, foundation) => current + ("\n" + indent + "  Foundation: " + foundation));

			// Output tableaus
			result = Tableaus.Aggregate(result, (current, tableau) => current + ("\n" + indent + "  Tableau: " + tableau));

			if (printMoves && recursionCount > 0)
			{
				List<State> moves = GetMoves();

				result += "\n" + indent;
				result += "\n" + indent + "  Amount of possible moves: " + moves.Count;

				foreach (State currentState in moves)
				{
					result += "\n" + indent;

					result += "\n" + currentState.ToString(true, recursionCount - 1, indent + "   |");
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

			int stateNumber = 1;

			// State where next Cards are moved to and from the waste
			if (Stock.IsEmpty)
			{
				State wasteToStock = new State(this)
				{
					PreviousState = this,
					CurrentStateNumber = stateNumber
				};
				wasteToStock.Stock.Waste.Empty();

				if (!wasteToStock.IsRepeatedState)
				{
					result.Add(wasteToStock);
					++stateNumber;
				}
			}
			else
			{
				State stockToWaste = new State(this)
				{
					PreviousState = this,
					CurrentStateNumber = stateNumber
				};
				stockToWaste.Stock.MoveToWaste();

				if (!stockToWaste.IsRepeatedState)
				{
					result.Add(stockToWaste);
					++stateNumber;
				}
			}

			// All possible card movements
			foreach (CardStack sourceStack in CardStacks)
			{
				if (!sourceStack.IsEmpty && sourceStack.CanRemoveCardFromTop())
				{
					// Prevent nearly identical moves
					bool foundFoundation = false;

					foreach (CardStack targetStack in CardStacks)
					{
						// Prevent moving more than one card to or from a foundation
						foreach (Card movableCard in sourceStack is FoundationCardStack || targetStack is FoundationCardStack ? new List<Card> { sourceStack.TopCard } : sourceStack.MovableCards)
						{
							// Prevent moving cards between empty stacks and itself
							bool isUselessMove = sourceStack == targetStack || movableCard == sourceStack.BottomCard && targetStack.IsEmpty;

							if (!isUselessMove && targetStack.CanPlaceCardOnTop(movableCard))
							{
								// Prevent nearly identical moves
								if (targetStack is FoundationCardStack)
								{
									if (foundFoundation)
									{
										continue;
									}
									else
									{
										foundFoundation = true;
									}
								}

								// Clone state
								State newState = new State(this)
								{
									PreviousState = this,
									CurrentStateNumber = stateNumber
								};

								Card newStateMovableCard = newState.CardStacks[CardStacks.IndexOf(sourceStack)].Cards[sourceStack.Cards.IndexOf(movableCard)];

								// Make move in new state
								newState.CardStacks[CardStacks.IndexOf(sourceStack)].MoveCardsFromTop(newState.CardStacks[CardStacks.IndexOf(targetStack)], newStateMovableCard, false, false);

								// Add new state
								if (!newState.IsRepeatedState)
								{
									result.Add(newState);
									++stateNumber;
								}
							}
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Checks if the two states are the same.
		/// </summary>
		/// <param name="otherState">The state to compare to.</param>
		/// <returns>Whether the states are the same.</returns>
		public bool IsSame(State otherState)
		{
			for (int stackIndex = 0; stackIndex < CardStacks.Count; ++stackIndex)
			{
				if (CardStacks[stackIndex].CardCount != otherState.CardStacks[stackIndex].CardCount)
				{
					return false;
				}

				for (int cardIndex = 0; cardIndex < CardStacks[stackIndex].CardCount; ++cardIndex)
				{
					Card currentCard = CardStacks[stackIndex].Cards[cardIndex];
					Card otherCard = otherState.CardStacks[stackIndex].Cards[cardIndex];
					if ((int)currentCard.Rank != (int)otherCard.Rank || (int)currentCard.Suit != (int)otherCard.Suit || currentCard.Flipped != otherCard.Flipped)
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}