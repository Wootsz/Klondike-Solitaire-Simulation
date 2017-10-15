using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
	public class State
	{
		public List<Card> deck = GenerateStandardDeck();
		int no_of_tableaus = 7;

		public Stack<Card> stock = new Stack<Card>();
        public Stack<Card> waste = new Stack<Card>();
		public Stack<Card> foundation_diamonds = new Stack<Card>();
		public Stack<Card> foundation_spades = new Stack<Card>();
		public Stack<Card> foundation_clubs = new Stack<Card>();
		public Stack<Card> foundation_hearts = new Stack<Card>();
		public List<Stack<Card>> tableaus = new List<Stack<Card>>();

		/// <summary>
		/// Generates a default deck with all 52 cards.
		/// </summary>
		/// <returns>A standard deck.</returns>
		public static List<Card> GenerateStandardDeck()
		{
			List<Card> result = new List<Card>();

			// Make a deck with every card
			for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++)
			{
				for (int j = 0; j < Enum.GetNames(typeof(Rank)).Length; j++)
				{
					result.Add(new Card((Suit)i, (Rank)j));
				}
			}

			return result;
		}

		/// <summary>
		/// Creates a new game state.
		/// </summary>
		public State()
		{
			ShuffleDeck();

			// Fill tableaus
			for (int i = 1; i <= no_of_tableaus; i++)
			{
				Stack<Card> s = new Stack<Card>();
				for (int n = 0; n < i; n++)
				{
					s.Push(deck.First());
					deck.RemoveAt(0);
				}

				tableaus.Add(s);
			}

			// Whatever's left, goes into the stock
			stock = deck;
		}

		/// <summary>
		/// Shuffles a deck.
		/// </summary>
		public void ShuffleDeck()
		{
			
		}

		/// <summary>
		/// Gets a string representation of the current state.
		/// </summary>
		/// <returns>The current state as a string.</returns>
		public override string ToString() {
			string output = "";

			// Output deck
			output += "Deck: " + String.Join(", ", deck);

			return output;
		}
	}
}
