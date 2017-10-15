using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Onderzoeksmethoden.Suit;
using static Onderzoeksmethoden.Rank;

namespace Onderzoeksmethoden
{
	public enum Suit {
		Spades,
		Hearts,
		Clubs,
		Diamonds
	}

	public enum Rank {
		A,
		_2,
		_3,
		_4,
		_5,
		_6,
		_7,
		_8,
		_9,
		_10,
		J,
		Q,
		K
	}

    class Card
    {
        public Suit suit;
        public Rank rank;

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }
    }
}
