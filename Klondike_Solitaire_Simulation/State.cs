using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
    class State
    {
        RNG random;
        public List<Card> deck;
        int no_of_tableaus = 7;

        public List<Card> stock;
        public Stack<Card> foundation_diamonds;
        public Stack<Card> foundation_spades;
        public Stack<Card> foundation_clubs;
        public Stack<Card> foundation_hearts;
        public List<Stack<Card>> tableaus;

        public State(RNG r)
        {
            this.random = r;

            // Make a deck with every card
           deck = new List<Card>();
            for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++)
                for (int j = 0; j < Enum.GetNames(typeof(Rank)).Length; j++)
                    deck.Add(new Card((Suit)i, (Rank)j));

            ShuffleDeck();

            // Fill tableaus
            for(int i = 1; i<= no_of_tableaus; i++)
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

        public void ShuffleDeck()
        {

        }
    }
}
