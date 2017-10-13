using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Onderzoeksmethoden.Suit;
using static Onderzoeksmethoden.Rank;

namespace Onderzoeksmethoden
{
    class Program
    {
        static void Main(string[] args)
        {
            int iterations = 1;

            for (int iteration = 0; iteration < iterations; iteration++)
            {

                RNG random = new RNG();

                // Make a deck with every card
                List<Card> deck = new List<Card>();
                for (int i = 0; i < Enum.GetNames(typeof(Suit)).Length; i++)
                    for (int j = 0; j < Enum.GetNames(typeof(Rank)).Length; j++)
                        deck.Add(new Card((Suit)i, (Rank)j));

                // TODO: shuffle deck

                foreach (Card c in deck)
                    Console.WriteLine(c.suit + " " + c.rank);



            }
            Console.ReadLine();
        }
    }
}
