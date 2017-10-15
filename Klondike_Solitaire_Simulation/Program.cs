using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Klondike_Solitaire_Simulation.Suit;
using static Klondike_Solitaire_Simulation.Rank;

namespace Klondike_Solitaire_Simulation
{
	class Program
	{
		static void Main(string[] args)
		{
			int iterations = 1;

            for (int iteration = 0; iteration < iterations; iteration++)
            {

                RNG random = new RNG();
                State init_state = new State(random);
                

				// TODO: shuffle deck

                


            }
            Console.ReadLine();
        }
    }
}
