﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Klondike_Solitaire_Simulation
{
	public class CardStack
	{
		private List<Card> cards = new List<Card>();

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
					result.Push(new Card((Suit)i, (Rank)j));
				}
			}

			return result;
		}

		/// <summary>
		/// Shuffles the stack.
		/// </summary>
		public void Shuffle()
		{
			while (cards.Count > 0)
			{
				int randomCard = Utility.random.Next(0, cards.Count);

				cards.Add(cards[randomCard]);
				cards.RemoveAt(randomCard);
			}
		}

		/// <summary>
		/// Adds a card to the stack.
		/// </summary>
		/// <param name="card">The card to add.</param>
		public void Push(Card card) {
			cards.Add(card);
		}

		/// <summary>
		/// Pops the top card off the stack.
		/// </summary>
		/// <returns>The top card.</returns>
		public Card Pop() {
			Card result = cards.Last();

			cards.Remove(cards.Last());

			return result;
		}

		/// <summary>
		/// Moves a specified amount of cards from the top of this stack to the other stack.
		/// </summary>
		/// <param name="otherStack">The other stack.</param>
		/// <param name="amount">The amount of cards to move.</param>
		public void MoveFromTop(CardStack otherStack, int amount) {
			for (int i = 0; i < amount; ++i) {
				otherStack.Push(Pop());
			}
		}
	}
}
