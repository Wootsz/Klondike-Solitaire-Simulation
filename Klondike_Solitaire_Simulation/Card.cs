﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
	public enum Suit
	{
		Spades,
		Hearts,
		Clubs,
		Diamonds
	}

	public enum Rank
	{
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

	public class Card
	{
		public Suit suit;
		public Rank rank;
		public Card attached;

		public Card(Suit suit, Rank rank)
		{
			this.suit = suit;
			this.rank = rank;
		}

		public Card TopCard()
		{
			if (attached == null)
			{
				return this;
			}
			else
			{
				return attached.TopCard();
			}
		}
	}
}
