using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Klondike_Solitaire_Simulation.Stacks
{
	public class CardStack
	{
		/// <summary>
		/// Counts the Cards in the card stack
		/// </summary>
		public int CardCount => Cards.Count;

		/// <summary>
		/// Amount of flipped Cards.
		/// </summary>
		public int FlippedCardCount
		{
			get
			{
				int result = 0;

				foreach (Card card in Cards)
				{
					if (card.Flipped)
					{
						++result;
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Amount of normal Cards.
		/// </summary>
		public int NormalCardCount => CardCount - FlippedCardCount;

		/// <summary>
		/// The maximum capacity of this stack.
		/// </summary>
		public int Capacity;

		/// <summary>
		/// The Cards on the stack.
		/// </summary>
		public List<Card> Cards = new List<Card>();

		/// <summary>
		/// All flipped cards, from lowest to highest.
		/// </summary>
		public List<Card> FlippedCards
		{
			get
			{
				List<Card> result = new List<Card>();

				foreach (Card card in Cards)
				{
					if (card.Flipped)
					{
						result.Add(card);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// All normal cards, from lowest to highest.
		/// </summary>
		public List<Card> NormalCards
		{
			get
			{
				List<Card> result = new List<Card>();

				foreach (Card card in Cards)
				{
					if (!card.Flipped)
					{
						result.Add(card);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// All cards that can be moved.
		/// </summary>
		public virtual List<Card> MovableCards => NormalCards;

		/// <summary>
		/// The bottom card.
		/// </summary>
		public Card BottomCard {
			get {
				if (IsEmpty())
				{
					throw new Exception("Card stack is empty!");
				}

				return Cards.First();
			}
		}

		/// <summary>
		/// Peeks at the top card.
		/// </summary>
		/// <returns>The top card.</returns>
		public Card TopCard
		{
			get
			{
				if (IsEmpty())
				{
					throw new Exception("Card stack is empty!");
				}

				return Cards.Last();
			}
		}

		/// <summary>
		/// Generates a default deck with all 52 Cards.
		/// </summary>
		/// <returns>A standard deck.</returns>
		public static CardStack GenerateStandardDeck()
		{
			CardStack result = new CardStack();

			// Make a deck with every card
			for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++)
			{
				for (int j = 0; j < Enum.GetNames(typeof(Rank)).Length; j++)
				{
					result.AddCardToTop(new Card((Suit)i, (Rank)j));
				}
			}

			return result;
		}

		/// <summary>
		/// Creates a new empty card stack.
		/// </summary>
		/// <param name="capacity">The maximum capacity of this stack.</param>
		public CardStack(int capacity = -1)
		{
			Capacity = capacity;
		}

		/// <summary>
		/// Copies the card stack.
		/// </summary>
		/// <param name="original">The stack to copy.</param>
		public CardStack(CardStack original)
		{
			for (int cardIndex = 0; cardIndex < original.CardCount; ++cardIndex)
			{
				Cards.Add(new Card(original.Cards[cardIndex]));
			}
			Capacity = original.Capacity;
		}

		/// <summary>
		/// Checks if the stack is empty.
		/// </summary>
		/// <returns>Whether the stack is empty.</returns>
		public bool IsEmpty()
		{
			return Cards.Count == 0;
		}

		/// <summary>
		/// Checks if the stack is full.
		/// </summary>
		/// <returns>Whether the stack is full.</returns>
		public bool IsFull()
		{
			return Capacity != -1 && CardCount >= Capacity;
		}

		/// <summary>
		/// Shuffles the stack.
		/// </summary>
		public CardStack ShuffleCards()
		{
			List<Card> newCards = new List<Card>();

			while (Cards.Count > 0)
			{
				int randomCard = Utility.random.Next(0, Cards.Count);

				newCards.Add(Cards[randomCard]);
				Cards.RemoveAt(randomCard);
			}

			Cards = newCards;

			return this;
		}

		/// <summary>
		/// Adds a card to the stack.
		/// </summary>
		/// <param name="card">The card to add.</param>
		public CardStack AddCardToTop(Card card)
		{
			if (IsFull())
			{
				throw new Exception("Card stack is full!");
			}

			Cards.Add(card);

			return this;
		}

		/// <summary>
		/// Pops the top card off the stack.
		/// </summary>
		/// <returns>The top card.</returns>
		public Card RemoveTopCard()
		{
			if (IsEmpty())
			{
				throw new Exception("Card stack is empty!");
			}

			Card result = Cards.Last();

			Cards.Remove(Cards.Last());

			return result;
		}

		/// <summary>
		/// Pushes another stack on top of this one.
		/// </summary>
		/// <param name="stack">The stack to add.</param>
		public CardStack AddStackToTop(CardStack stack)
		{
			if (IsFull())
			{
				throw new Exception("Card stack is full!");
			}

			Cards.AddRange(stack.Cards);

			return this;
		}

		/// <summary>
		/// Flips all Cards in the stack.
		/// </summary>
		public CardStack FlipAllCards()
		{
			foreach (Card card in Cards)
			{
				card.Flip();
			}

			return this;
		}

		/// <summary>
		/// Gets the topmost flipped card.
		/// </summary>
		/// <returns>The topmost flipped card.</returns>
		public Card GetTopmostFlippedCard()
		{
			for (int i = CardCount - 1; i >= 0; --i)
			{
				if (Cards[i].Flipped)
				{
					return Cards[i];
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the lowest normal card.
		/// </summary>
		/// <returns>the lowest normal card.</returns>
		public Card GetLowestNormalCard()
		{
			for (int i = 0; i < CardCount; ++i)
			{
				if (!Cards[i].Flipped)
				{
					return Cards[i];
				}
			}

			return null;
		}

		/// <summary>
		/// Moves a specified amount of Cards from the top of this stack to the other stack.
		/// </summary>
		/// <param name="otherStack">The other stack.</param>
		/// <param name="amount">The amount of Cards to move.</param>
		/// <param name="flip">Whether to flip the Cards as they are moved.</param>
		/// <param name="flipNext">Whether to flip the next card after all Cards were moved.</param>
		/// <param name="reverse">Whether to reverse the order of the Cards.</param>
		public virtual CardStack MoveCardsFromTop(CardStack otherStack, int amount, bool flip = false, bool reverse = true)
		{
			for (int i = 0; i < amount; ++i)
			{
				Card card = null;
				if (reverse)
				{
					card = RemoveTopCard();
				}
				else
				{
					card = Cards[Cards.Count - amount + i];
					Cards.Remove(card);
				}

				if (flip)
				{
					card.Flip();
				}

				otherStack.AddCardToTop(card);
			}

			return this;
		}

		public CardStack MoveCardsFromTop(CardStack otherCardStack, Card card, bool flip = false, bool reverse = true)
		{
			return MoveCardsFromTop(otherCardStack, CardCount - Cards.IndexOf(card), flip, reverse);
		}

		/// <summary>
		/// Checks whether a move is possible.
		/// </summary>
		/// <param name="card">The card to place.</param>
		/// <returns>Whether the move is possible.</returns>
		public virtual bool CanPlaceCardOnTop(Card card)
		{
			return !IsFull();
		}

		/// <summary>
		/// Checks whether it is possible to remove a card from the top.
		/// </summary>
		/// <returns>Whether it is possible.</returns>
		public virtual bool CanRemoveCardFromTop()
		{
			return !IsEmpty();
		}

		/// <summary>
		/// Gets the card stack as a string.
		/// </summary>
		/// <returns>The card stack as a string.</returns>
		public override string ToString() => String.Join(", ", Cards);
	}
}
