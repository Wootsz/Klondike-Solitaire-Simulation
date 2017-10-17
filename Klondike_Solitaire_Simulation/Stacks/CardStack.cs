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
		/// Counts the cards in the card stack
		/// </summary>
		public int CardCount => cards.Count;

		/// <summary>
		/// Amount of flipped cards.
		/// </summary>
		public int FlippedCardCount {
			get {
				int result = 0;

				foreach (Card card in cards) {
					if (card.Flipped) {
						++result;
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Amount of normal cards.
		/// </summary>
		public int NormalCardCount => CardCount - FlippedCardCount;

		/// <summary>
		/// The maximum capacity of this stack.
		/// </summary>
		public int Capacity;

		/// <summary>
		/// The cards on the stack.
		/// </summary>
		protected List<Card> cards = new List<Card>();

		/// <summary>
		/// Generates a default deck with all 52 cards.
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
				cards.Add(new Card(original.cards[cardIndex]));
			}
			Capacity = original.Capacity;
		}

		/// <summary>
		/// Checks if the stack is empty.
		/// </summary>
		/// <returns>Whether the stack is empty.</returns>
		public bool IsEmpty()
		{
			return cards.Count == 0;
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

			while (cards.Count > 0)
			{
				int randomCard = Utility.random.Next(0, cards.Count);

				newCards.Add(cards[randomCard]);
				cards.RemoveAt(randomCard);
			}

			cards = newCards;

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

			cards.Add(card);

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

			Card result = cards.Last();

			cards.Remove(cards.Last());

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

			cards.AddRange(stack.cards);

			return this;
		}

		/// <summary>
		/// Peeks at the top card.
		/// </summary>
		/// <returns>The top card.</returns>
		public Card PeekAtTopCard()
		{
			if (IsEmpty())
			{
				throw new Exception("Card stack is empty!");
			}

			return cards.Last();
		}

		/// <summary>
		/// Flips all cards in the stack.
		/// </summary>
		public CardStack FlipAllCards()
		{
			foreach (Card card in cards)
			{
				card.Flip();
			}

			return this;
		}

		/// <summary>
		/// Moves a specified amount of cards from the top of this stack to the other stack.
		/// </summary>
		/// <param name="otherStack">The other stack.</param>
		/// <param name="amount">The amount of cards to move.</param>
		/// <param name="flip">Whether to flip the cards as they are moved.</param>
		/// <param name="flipNext">Whether to flip the next card after all cards were moved.</param>
		public CardStack MoveCardsFromTop(CardStack otherStack, int amount, bool flip = false, bool flipNext = false)
		{
			for (int i = 0; i < amount; ++i)
			{
				Card card = RemoveTopCard();

				if (flip)
				{
					card.Flip();
				}

				otherStack.AddCardToTop(card);
			}

			if (flipNext && !IsEmpty())
			{
				PeekAtTopCard().Flip();
			}

			return this;
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
		public override string ToString() => String.Join(", ", cards);
	}
}
