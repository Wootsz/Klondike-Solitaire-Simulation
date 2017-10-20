using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klondike_Solitaire_Simulation.Heuristics
{
    class RandomHeuristic : BaseHeuristic
    {
        public RandomHeuristic(RNG r) : base(r)
        {
        }

        public override State GetMove(State currentState, List<State> moves)
        {
            return moves[(int)r.Next(moves.Count)];
        }


    }
}
